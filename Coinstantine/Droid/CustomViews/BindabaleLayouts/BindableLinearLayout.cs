using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Exceptions;
using MvvmCross.Platforms.Android.Binding.BindingContext;
using MvvmCross.Platforms.Android.Binding.Views;

namespace Coinstantine.Droid.CustomViews.BindableLayouts
{
    public class BindableLinearLayout : LinearLayout, IDataContextProvider, IMvxBindingContextOwner
    {
        private readonly int _templateId;
        private readonly IMvxAndroidBindingContext _bindingContext;

        public BindableLinearLayout(Context context, IAttributeSet attrs)
            : this(MvxAttributeHelpers.ReadTemplateId(context, attrs), context, attrs)
        {
        }

        public BindableLinearLayout(int templateId, Context context, IAttributeSet attrs)
            : base(context, attrs)
        {
            _templateId = templateId;

            if (!(context is IMvxLayoutInflaterHolder))
            {
                throw new MvxException("The owning Context for a MvxFrameControl must implement LayoutInflater");
            }

            _bindingContext = new MvxAndroidBindingContext(context, (IMvxLayoutInflaterHolder)context);
            this.DelayBind(() =>
            {
                if (Content == null && _templateId != 0)
                {
                    Content = _bindingContext.BindingInflate(_templateId, this);
                }
            });
        }

        protected BindableLinearLayout(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public BindableLinearLayout(Context context) : base(context)
        {
            _bindingContext = new MvxAndroidBindingContext(context, (IMvxLayoutInflaterHolder)context);
        }

        protected IMvxAndroidBindingContext AndroidBindingContext => _bindingContext;

        public IMvxBindingContext BindingContext
        {
            get { return _bindingContext; }
            set { throw new NotImplementedException("BindingContext is readonly in the list item"); }
        }

        protected object _cachedDataContext;
        private bool _isAttachedToWindow;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.ClearAllBindings();
                _cachedDataContext = null;
            }

            base.Dispose(disposing);
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();
            _isAttachedToWindow = true;
            if (_cachedDataContext != null
                && DataContext == null)
            {
                DataContext = _cachedDataContext;
            }
        }

        protected override void OnDetachedFromWindow()
        {
            _cachedDataContext = DataContext;
            DataContext = null;
            base.OnDetachedFromWindow();
            _isAttachedToWindow = false;
        }

        private View _content;

        protected View Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnContentSet();
            }
        }

        protected virtual void OnContentSet()
        {
        }

        [MvxSetToNullAfterBinding]
        public object DataContext
        {
            get
            {
                return _bindingContext.DataContext;
            }
            set
            {
                if (_isAttachedToWindow)
                {
                    _bindingContext.DataContext = value;
                }
                else
                {
                    _cachedDataContext = value;
                    if (_bindingContext.DataContext != null)
                    {
                        _bindingContext.DataContext = null;
                    }
                }
            }
        }
    }
}