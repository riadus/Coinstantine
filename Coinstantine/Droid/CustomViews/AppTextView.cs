using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Text;
using Android.Text.Style;
using Android.Util;
using Android.Views;
using Android.Webkit;
using Coinstantine.Common;
using Java.Lang;

namespace Coinstantine.Droid.CustomViews
{
    public class JustifiedTextView : WebView
    {
        public JustifiedTextView(Context context) : base(context)
        {
        }

        public JustifiedTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }
    }

    public class AppTextView : AppCompatTextView
    {
        //Object that helps us to measure the words and characters like spaces.
        private Paint _paint;

        //Thin space (Hair Space actually) character that will fill the spaces
        private readonly char _thinSpace = '\u200A';

        //string that will storage the text with the inserted spaces
        private string _justifiedText = "";

        //Float that represents the actual width of a sentence
        private float _sentenceWidth = 0;

        //Integer that counts the spaces needed to fill the line being processed
        private int _whiteSpacesNeeded = 0;

        //Integer that counts the actual amount of words in the sentence
        private int _wordsInThisSentence = 0;

        //ArrayList of strings that will contain the words of the sentence being processed
        private List<string> _temporalLine = new List<string>();

        //stringBuilder that will hold the temporal chunk of the string to calculate word index.
        private StringBuilder _stringBuilderCSequence = new StringBuilder();

        //List of SpanHolder class that will hold the spans within the giving string.
        private List<SpanHolder> _spanHolderList = new List<SpanHolder>();

        //stringBuilder that will store temp data for joining sentence.
        private StringBuilder _sentence = new StringBuilder();

        private int _viewWidth;

        private float _thinSpaceWidth;

        private float _whiteSpaceWidth;

        public AppTextView(Context context) : base(context)
        {
        }

        public AppTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public AppTextView(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        protected AppTextView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
        {
            base.OnLayout(changed, left, top, right, bottom);

            if (_justifiedText.Replace(" ", "")
                    .Replace("", _thinSpace.ToString())
                    .Equals(Text.Replace(" ", "").Replace("", _thinSpace.ToString())))
            {
                return;
            }

            var layoutParams = LayoutParameters;

            ICharSequence charSequence = new Java.Lang.String(Text);

            _spanHolderList.Clear();

            var words = Text.Split(' ');

            //Get spans within the string and adds the instance references into the
            //SpanHolderList to be applied once the justify process has been performed.

            var s = SpannableString.ValueOf(charSequence);
            if (charSequence is SpannedString spannedString)
            {
                for (int i = 0; i < Text.Length - 1; i++)
                {
                    CharacterStyle[] spans = spannedString.GetSpans(i, i + 1, Class.FromType(typeof(CharacterStyle))) as CharacterStyle[];
                    if (spans != null && spans.Length > 0)
                    {
                        foreach (var span in spans)
                        {
                            int spaces =
                                    charSequence.ToString().Substring(0, i).Split(' ').Length + charSequence.ToString()
                                            .Substring(0, i)
                                                .Split(_thinSpace).Length;

                            var spanHolder = new SpanHolder(spans, s.GetSpanStart(span), s.GetSpanEnd(span), spaces);
                            _stringBuilderCSequence.SetLength(0);
                            for (int j = 0; j <= words.Length - 1; j++)
                            {
                                _stringBuilderCSequence.Append(words[j]);
                                _stringBuilderCSequence.Append(" ");
                                if (_stringBuilderCSequence.Length() > i)
                                {
                                    if (words[j].Trim().Replace(_thinSpace.ToString(), "").Length == 1)
                                    {


                                        spanHolder.WordHolderIndex = j;
                                    }
                                    else
                                    {
                                        spanHolder.WordHolderIndex = j;
                                        spanHolder.IsTextChunkPadded = true;
                                    }
                                    break;
                                }
                            }
                            _spanHolderList.Add(spanHolder);
                        }
                    }
                }
            }
            _paint = Paint;
            _viewWidth = MeasuredWidth - PaddingLeft + PaddingRight;

            //This class won't justify the text if the TextView has wrap_content as width
            //And won't repeat the process of justify text if it's already done.
            //AND! won't justify the text if the view width is 0
            if (layoutParams.Width != ViewGroup.LayoutParams.WrapContent
                    && _viewWidth > 0
                    && words.Length > 0
                    && _justifiedText.IsNullOrEmpty())
            {
                _thinSpaceWidth = _paint.MeasureText(_thinSpace.ToString());
                _whiteSpaceWidth = _paint.MeasureText(" ");
                for (int i = 0; i <= words.Length - 1; i++)
                {
                    var containsNewLine = words[i].Contains("\n") || words[i].Contains("\r");
                    if (containsNewLine)
                    {
                        string[] splitted = words[i].Split(new[] { "(?<=\\n)" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var splitWord in splitted)
                        {
                            ProcessWord(splitWord, splitWord.Contains("\n"));
                        }
                    }
                    else
                    {
                        ProcessWord(words[i], false);
                    }
                }
                _justifiedText += JoinWords(_temporalLine);
            }

            //Apply the extra spaces to the items of the SpanList that were added due
            //the justifying process.
            var spannableString = SpannableString.ValueOf(_justifiedText);

            foreach (var sH in _spanHolderList)
            {
                int spaceCount = 0, wordCount = 0;
                var isCountingWord = false;
                int j = 0;
                while (wordCount < (sH.WordHolderIndex + 1))
                {
                    if (_justifiedText[j] == ' ' || _justifiedText[j] == ' ')
                    {
                        spaceCount++;
                        if (isCountingWord)
                        {
                            wordCount++;
                        }
                        isCountingWord = false;
                    }
                    else
                    {
                        isCountingWord = true;
                    }
                    j++;
                }
                sH.Start = sH.Start + spaceCount - sH.CurrentSpaces + (sH.IsTextChunkPadded ? 1 : 0);
                sH.End =
                        sH.End + spaceCount - sH.CurrentSpaces + (sH.IsTextChunkPadded ? 1 : 0);
            }
            //Applies spans on Justified string.
            foreach (var sH in _spanHolderList)
            {
                foreach (var cS in sH.Spans)
                    spannableString.SetSpan(cS, sH.Start, sH.End, 0);
            }

            if (_justifiedText.IsNotNull())
            {
                Text = spannableString.ToString();
            }
        }

        private void ProcessWord(string word, bool containsNewLine)
        {
            if ((_sentenceWidth + _paint.MeasureText(word)) < _viewWidth)
            {
                _temporalLine.Add(word);
                _wordsInThisSentence++;
                _temporalLine.Add(containsNewLine ? "" : " ");
                _sentenceWidth += _paint.MeasureText(word) + _whiteSpaceWidth;
                if (containsNewLine)
                {
                    _justifiedText += JoinWords(_temporalLine);
                    ResetLineValues();
                }
            }
            else
            {
                while (_sentenceWidth < _viewWidth)
                {
                    _sentenceWidth += _thinSpaceWidth;
                    if (_sentenceWidth < _viewWidth) _whiteSpacesNeeded++;
                }

                if (_wordsInThisSentence > 1)
                {
                    InsertWhiteSpaces(_whiteSpacesNeeded, _wordsInThisSentence, _temporalLine);
                }
                _justifiedText += JoinWords(_temporalLine);
                ResetLineValues();

                if (containsNewLine)
                {
                    _justifiedText += word;
                    _wordsInThisSentence = 0;
                    return;
                }
                _temporalLine.Add(word);
                _wordsInThisSentence = 1;
                _temporalLine.Add(" ");
                _sentenceWidth += _paint.MeasureText(word) + _whiteSpaceWidth;
            }
        }

        //Method that resets the values of the actual line being processed
        private void ResetLineValues()
        {
            _temporalLine.Clear();
            _sentenceWidth = 0;
            _whiteSpacesNeeded = 0;
            _wordsInThisSentence = 0;
        }

        //Function that joins the words of the ArrayList
        private string JoinWords(List<string> words)
        {
            _sentence.SetLength(0);
            foreach (var word in words)
            {
                _sentence.Append(word);
            }
            return _sentence.ToString();
        }

        //Method that inserts spaces into the words to make them fix perfectly in the width of the view. I know I'm a genius naming stuff :)
        private void InsertWhiteSpaces(int whiteSpacesNeeded, int wordsInThisSentence,
                               List<string> sentence)
        {

            if (whiteSpacesNeeded == 0) return;

            if (whiteSpacesNeeded == wordsInThisSentence)
            {
                for (int i = 1; i < sentence.Count; i += 2)
                {
                    sentence[i] = sentence[i] + _thinSpace;
                }
            }
            else if (whiteSpacesNeeded < wordsInThisSentence)
            {
                for (int i = 0; i < whiteSpacesNeeded; i++)
                {
                    int randomPosition = GetRandomEvenNumber(sentence.Count - 1);
                    sentence[randomPosition] = sentence[randomPosition] + _thinSpace;
                }
            }
            else if (whiteSpacesNeeded > wordsInThisSentence)
            {
                //I was using recursion to achieve this... but when you tried to watch the preview,
                //Android Studio couldn't show any preview because a StackOverflow happened.
                //So... it ended like this, with a wild while xD.
                while (whiteSpacesNeeded > wordsInThisSentence)
                {
                    for (int i = 1; i < sentence.Count - 1; i += 2)
                    {
                        sentence[i] = sentence[i] + _thinSpace;
                    }
                    whiteSpacesNeeded -= (wordsInThisSentence - 1);
                }
                if (whiteSpacesNeeded == 0) return;

                if (whiteSpacesNeeded == wordsInThisSentence)
                {
                    for (int i = 1; i < sentence.Count; i += 2)
                    {
                        sentence[i] = sentence[i] + _thinSpace;
                    }
                }
                else if (whiteSpacesNeeded < wordsInThisSentence)
                {
                    for (int i = 0; i < whiteSpacesNeeded; i++)
                    {
                        var randomPosition = GetRandomEvenNumber(sentence.Count - 1);
                        sentence[randomPosition] = sentence[randomPosition] + _thinSpace;
                    }
                }
            }

        }
        //Gets a random number, it's part of the algorithm... don't blame me.
        private int GetRandomEvenNumber(int max)
        {
            Random rand = new Random();

            // nextInt is normally exclusive of the top value,
            return rand.Next(max) & ~1;
        }
    }

    public class SpanHolder
    {
        public SpanHolder(CharacterStyle[] spans, int start, int end, int spaces)
        {
            Spans = spans;
            Start = start;
            End = end;
            CurrentSpaces = spaces;
        }

        public bool IsTextChunkPadded { get; set; }
        public int WordHolderIndex { get; set; }
        public CharacterStyle[] Spans { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public int CurrentSpaces { get; set; }
    }
}
