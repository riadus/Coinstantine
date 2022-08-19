namespace Coinstantine.Domain.Interfaces.Translations
{
    public static class TranslationKeys
    {
        public static class Home
        {
            public static TranslationKey BuyCsn = "Home_BuyCSN";
            public static TranslationKey GoBuyCsn = "Home_GoBuyCSN";
            public static TranslationKey Airdrop = "Home_Airdrop";
            public static TranslationKey AvailableLater = "Home_AvailableLater";
            public static TranslationKey RefreshingMessage = "Home_RefreshingMessage";
            public static TranslationKey RefreshingListMessage = "Home_RefreshingListMessage";
            public static TranslationKey Subscribe = "Home_Subscribe";
            public static TranslationKey Details = "Home_Details";
            public static TranslationKey SelectShareOption = "Home_SelectShareOption";
            public static TranslationKey SendShareOption = "Home_SendShareOption";
            public static TranslationKey CopyShareOption = "Home_CopyShareOption";
            public static TranslationKey AddressCopied = "Home_AddressCopied";
        }

        public static class Airdrop
        {
            public static TranslationKey TwitterAccount = "Airdrops_TwitterAccount";
            public static TranslationKey TwitterFollowers = "Airdrops_TwitterFollowers";
            public static TranslationKey TwitterCreationDate = "Airdrops_TwitterCreationDate";
            public static TranslationKey TelegramAccount = "Airdrops_TelegramAccount";
            public static TranslationKey BctAccount = "Airdrops_BctAccount";
            public static TranslationKey BctExactPosition = "Airdrops_BctExactPosition";
            public static TranslationKey BctMinimumPosition = "Airdrops_BctMinimumPosition";
            public static TranslationKey BctMinimumPosts = "Airdrops_BctMinimumPosts";
            public static TranslationKey BctMinimumActivity = "Airdrops_BctMinimumActivity";
            public static TranslationKey BctCreationDate = "Airdrops_BctCreationDate";

            public static TranslationKey AvailableOn = "Airdrops_AvailableOn";
            public static TranslationKey ExpiresOn = "Airdrops_ExpiresOn";
            public static TranslationKey Expired = "Airdrops_Expired";
            public static TranslationKey NotAvailableAnymore = "Airdrops_NotAvailableAnymore";
            public static TranslationKey NotAvailableSoon = "Airdrops_NotAvailableSoon";

            public static TranslationKey Description = "Airdrops_Description";
            public static TranslationKey AirdropName = "Airdrops_AirdropName";
            public static TranslationKey TokenName = "Airdrops_TokenName";
            public static TranslationKey Amount = "Airdrops_Amount";
            public static TranslationKey MaxParticipants = "Airdrops_MaxParticipants";
            public static TranslationKey NumberOfParticipants = "Airdrops_NumberOfParticipants";
            public static TranslationKey StartDate = "Airdrops_StartDate";
            public static TranslationKey EndDate = "Airdrops_EndDate";
            public static TranslationKey Requirements = "Airdrops_Requirements";

            public static TranslationKey Subscribe = "Airdrops_Subscribe";
            public static TranslationKey AlreadySubscribed = "Airdrops_AlreadySubscribed";
            public static TranslationKey RequirementsNotMet = "Airdrops_RequirementsNotMet";
            public static TranslationKey NotStartedYet = "Airdrops_NotStartedYet";
            public static TranslationKey ExpiredDetail = "Airdrops_ExpiredDetail";
            public static TranslationKey Full = "Airdrops_Full";
            public static TranslationKey InformationToBeShared = "Airdrops_InformationToBeShared";
            public static TranslationKey AlertBeforeSending = "Airdrops_AlertBeforeSending";
            public static TranslationKey Success = "Airdrops_Success";
            public static TranslationKey SuccessNoNotifications = "Airdrops_SuccessNoNotifications";
            public static TranslationKey Fail = "Airdrops_Fail";
            public static TranslationKey UnknownAirdrop = "Airdrops_UnknownAirdrop";
            public static TranslationKey Subscribing = "Airdrops_Subscribing";
        }

        public static class Buy
        {
            public static TranslationKey CsnValue = "Buy_CsnValue";
            public static TranslationKey CsnAmount = "Buy_CsnAmount";
            public static TranslationKey CsnCost = "Buy_CsnCost";
            public static TranslationKey Bonus = "Buy_Bonus";
            public static TranslationKey NotEnoughETH = "Buy_NotEnoughETH";
            public static TranslationKey MaxBuyableCsn = "Buy_MaxBuyableCsn";
            public static TranslationKey AlertBuy = "Buy_Alert";
            public static TranslationKey AlertBuyError = "Buy_Error";
            public static TranslationKey AlertBuyErrorSmartContract = "Buy_Error_SmartContract";
            public static TranslationKey AlertBuySuccess = "Buy_Successful";
            public static TranslationKey AlertBuyPending = "Buy_Pending";
            public static TranslationKey ShareHash = "Buy_ShareHash";
            public static TranslationKey Presale = "Buy_Presale";
            public static TranslationKey MinimumEtherRequired = "Buy_MinimumEtherRequired";
            public static TranslationKey SendingTransaction = "Buy_SendingTransaction"; 
            public static TranslationKey PricesNotSynced = "Buy_PricesNotSynced";
            public static TranslationKey PresaleNotOpenYet = "Buy_PresaleNotOpenYet";
            public static TranslationKey PresaleClosed = "Buy_PresaleClosed";
        }

        public static class LandingPage
        {
            public static TranslationKey GetStarted = "Fixed_LandingPage_GetStarted";
            public static TranslationKey Login = "Fixed_LandingPage_Login";
            public static TranslationKey Loading = "Fixed_LandingPage_Loading";
        }

        public static class MainMenu
        {
            public static TranslationKey Wallet = "MainMenu_Wallet";
            public static TranslationKey Airdrops = "MainMenu_Aidrops";
            public static TranslationKey ValidateProfile = "MainMenu_ValidateProfile";
            public static TranslationKey Settings = "MainMenu_Settings";
            public static TranslationKey Menu = "MainMenu_Menu";
            public static TranslationKey ConfirmLogout = "MainMenu_ConfirmLogout";
        }

        public static class Profile
        {
            public static TranslationKey Twitter = "Profile_Twitter";
            public static TranslationKey Telegram = "Profile_Telegram";
            public static TranslationKey BitcoinTalkUserId = "Profile_BitcoinTalkUserId";
            public static TranslationKey SetProfile = "Profile_SetProfile";

            public static TranslationKey PhoneExplanation = "Profile_PhoneExplanation";
            public static TranslationKey TwitterExplanation = "Profile_TwitterExplanation";
            public static TranslationKey TelegramExplanation = "Profile_TelegramExplanation";
            public static TranslationKey BitcoinTalkExplanation = "Profile_BitcoinTalkExplanation";
        }

        public static class UserAccount
        {
            public static TranslationKey Next = "Fixed_UserAccount_Next";
            public static TranslationKey Submit = "Fixed_UserAccount_Submit";
            public static TranslationKey Username = "Fixed_UserAccount_Username";
            public static TranslationKey Email = "Fixed_UserAccount_Email";
            public static TranslationKey Password = "Fixed_UserAccount_Password";
            public static TranslationKey Address = "Fixed_UserAccount_Address";
            public static TranslationKey City = "Fixed_UserAccount_City";
            public static TranslationKey Country = "Fixed_UserAccount_Country";
            public static TranslationKey FirstName = "Fixed_UserAccount_FirstName";
            public static TranslationKey LastName = "Fixed_UserAccount_LastName";
            public static TranslationKey DateOfBirth = "Fixed_UserAccount_DateOfBirth";
            public static TranslationKey LoginFailed = "Fixed_UserAccount_LoginFailed";
            public static TranslationKey AccountNotConfirmed = "Fixed_UserAccount_AccountNotConfirmed";
            public static TranslationKey EmailTaken = "Fixed_UserAccount_EmailTaken";
            public static TranslationKey UsernameTaken = "Fixed_UserAccount_UsernameTaken";
            public static TranslationKey PasswordInvalid = "Fixed_UserAccount_PasswordInvalid";
            public static TranslationKey DateOfBirthInvalid = "Fixed_UserAccount_DateOfBirthInvalid";
            public static TranslationKey EmailInvalid = "Fixed_UserAccount_EmailInvalid";
            public static TranslationKey PasswordPattern = "Fixed_UserAccount_PasswordPattern";
            public static TranslationKey AccountCreated = "Fixed_UserAccount_AccountCreated";
            public static TranslationKey ErrorWhileCreatingAccount = "Fixed_UserAccount_ErrorWhileCreatingAccount";
            public static TranslationKey ForgotCredentials = "Fixed_UserAccount_ForgotCredentials";
        }

        public static class Settings
        {
            public static TranslationKey ChangeLanguage = "Settings_ChangeLanguage";
            public static TranslationKey ResetPincode = "Settings_ResetPincode";
            public static TranslationKey About = "Settings_About";
            public static TranslationKey TermsAndConditions = "Settings_TermsAndConditions";

            public static TranslationKey Telegram = "Settings_Telegram";
            public static TranslationKey Twitter = "Settings_Twitter";
            public static TranslationKey BitcoinTalk = "Settings_BitcoinTalk";
        }

        public static class Pincode
        {
            public static TranslationKey SetNewPincode = "Pincode_SetNewPincode";
            public static TranslationKey SetPincode = "Pincode_SetPincode";
            public static TranslationKey CheckPincode = "Pincode_CheckPincode";
            public static TranslationKey UseBiometrics = "Pincode_UseBiometrics";
            public static TranslationKey WhyUseBiometrics = "Pincode_WhyUseBiometrics";
            public static TranslationKey WrongPincode = "Pincode_WrongPincode";
            public static TranslationKey SecondChance = "Pincode_SecondChance";
            public static TranslationKey LastChance = "Pincode_LastChance";
            public static TranslationKey TooManyWrongPincode = "Pincode_TooManyWrongPincode";
            public static TranslationKey AndroidFaceId = "Pincode_AndroidFaceId";
            public static TranslationKey AndroidFingerprint = "Pincode_AndroidFingerprint";
        }

        public static class Telegram
        {
            public static TranslationKey UsernameNeededError = "Telegram_UsernameNeededError";
            public static TranslationKey WatchTutorial = "Telegram_WatchTutorial";
            public static TranslationKey StartConversation = "Telegram_StartConversation";
            public static TranslationKey ExplanationText = "Telegram_ExplanationText";

            public static TranslationKey Username = "Telegram_Username";
            public static TranslationKey FirstName = "Telegram_FirstName";
            public static TranslationKey LastName = "Telegram_LastName";
            public static TranslationKey UserId = "Telegram_UserId";
            public static TranslationKey Alert = "Telegram_Alert";
            public static TranslationKey LoadingTelegram = "Telegram_LoadingTelegram";
            public static TranslationKey CouldNotCheckUser = "Telegram_CouldNotCheckUser";
            public static TranslationKey SettingProfile = "Telegram_SettingProfile";
            public static TranslationKey AccountNotValidated = "Telegram_AccountNotValidated";
        }

        public static class TelegramTutorial
        {
            public static TranslationKey Skip = "TelegramTutorial_Skip";
            public static TranslationKey Next = "TelegramTutorial_Next";
            public static TranslationKey Start = "TelegramTutorial_Start";

            public static TranslationKey Title1 = "TelergamTutorial_Title1";
            public static TranslationKey Title2 = "TelergamTutorial_Title2";
            public static TranslationKey Title3 = "TelergamTutorial_Title3";
            public static TranslationKey Title4 = "TelergamTutorial_Title4";

            public static TranslationKey Text1 = "TelergamTutorial_Text1";
            public static TranslationKey Text2 = "TelergamTutorial_Text2";
            public static TranslationKey Text3 = "TelergamTutorial_Text3";
            public static TranslationKey Text4 = "TelergamTutorial_Text4";
        }

        public static class Twitter
        {
            public static TranslationKey AuthenticateAndTweet = "Twitter_AuthenticateAndTweet";
            public static TranslationKey CheckAgain = "Twitter_CheckAgain";
            public static TranslationKey ExplanationText = "Twitter_Explanation";
            public static TranslationKey Alert = "Twitter_Alert";

            public static TranslationKey AccountNotValidated = "Twitter_TwitterAccountNotValidated";
            public static TranslationKey InfoMightBeShared = "Twitter_InfosMightBeShared";
            public static TranslationKey Followers = "Twitter_Followers";

            public static TranslationKey CreationDate = "Twitter_CreationDate";
            public static TranslationKey ScreenName = "Twitter_ScreenName";

            public static TranslationKey LoadingTwitter = "Twitter_LoadingTwitter";
            public static TranslationKey SettingProfile = "Twitter_SettingProfile";
            public static TranslationKey UpdatingProfile = "Twitter_UpdatingProfile";

            public static TranslationKey CouldNotTweet = "Twitter_CouldNotTweet";
        }

        public static class BitcoinTalk
        {
            public static TranslationKey CheckButtonText = "BCT_CheckButton";
            public static TranslationKey UserId = "BCT_UserId";
            public static TranslationKey ExplanationText = "BCT_Explanation";
            public static TranslationKey Alert = "BCT_Alert";

            public static TranslationKey Username = "BCT_Username";
            public static TranslationKey Posts = "BCT_Posts";
            public static TranslationKey Activity = "BCT_Activity";
            public static TranslationKey Rank = "BCT_Rank";
            public static TranslationKey RegistrationDate = "BCT_RegistrationDate";
            public static TranslationKey Location = "BCT_Location";
            public static TranslationKey UserNotFoud = "BCT_UserNotFound";

            public static TranslationKey SettingProfile = "BCT_SettingProfile";
            public static TranslationKey UpdatingProfile = "BCT_UpdatingProfile";
            public static TranslationKey GettingUser = "BCT_GettingUser";
            public static TranslationKey AccountNotValidated = "BCT_AccountNotValidated";
        }

        public static class General
        {
            public static TranslationKey Blank = "White_space";
            public static TranslationKey Coinstantine = "General_Coinstantine";
            public static TranslationKey Ether = "General_Ether";
            public static TranslationKey Ethereum = "General_Ethereum";
            public static TranslationKey Logout = "General_Logout";
            public static TranslationKey Username = "General_Username";
            public static TranslationKey BitcoinTalkAccount = "General_BitcoinTalkAccount";
            public static TranslationKey PhoneNumber = "General_PhoneNumber";
            public static TranslationKey TelegramAccount = "General_TelegramAccount";
            public static TranslationKey TwitterAccount = "General_TwitterAccount";
            public static TranslationKey NoLinkedAccount = "General_NoLinkedAccount";

            public static TranslationKey Oops = "General_Oops";
            public static TranslationKey Ok = "Fixed_General_Ok";
            public static TranslationKey Cancel = "General_Cancel";
            public static TranslationKey No = "General_No";
            public static TranslationKey Yes = "General_Yes";
            public static TranslationKey Loading = "General_Loading";
            public static TranslationKey NoInternet = "General_NoInternet";
            public static TranslationKey Confirm = "General_Confirm";
            public static TranslationKey Edit = "General_Edit";
            public static TranslationKey Update = "General_Update";
            public static TranslationKey OwnLanguage = "General_OwnLanguage";
            public static TranslationKey SessionExpired = "General_SessionExpired";
            public static TranslationKey ShareAddress = "General_ShareAddress";
            public static TranslationKey Syncing = "General_Syncing";
            public static TranslationKey NoEmailClient = "General_NoEmailClient";
            public static TranslationKey Canceling = "General_Canceling";
        }

        public static class Presale
        {
            public static TranslationKey Amount = "Presale_Amount";
            public static TranslationKey Value = "Presale_Value";
            public static TranslationKey Date = "Presale_Date";
            public static TranslationKey TransactionHash = "Presale_TransactionHash";
            public static TranslationKey Gas = "Presale_Gas";
            public static TranslationKey CumulativeGas = "Presale_CumulativeGas";
            public static TranslationKey PresaleParticipation = "Presale_PresaleParticipation";
            public static TranslationKey ShowTransaction = "Presale_ShowTransaction";
        }

        public static class About
        {
            public static TranslationKey Team = "About_Team";
            public static TranslationKey Company = "About_Company";
            public static TranslationKey Developer = "About_Developer";
            public static TranslationKey Designer = "About_Designer";
            public static TranslationKey Contact = "About_Contact";
            public static TranslationKey Framework = "About_Framework";
            public static TranslationKey Technologies = "About_Technologies";
            public static TranslationKey CrossPlatform = "About_CrossPlatform";
            public static TranslationKey Animations = "About_Animations";
            public static TranslationKey Tutorial = "About_Tutorial";
            public static TranslationKey Backend = "About_Backend";
            public static TranslationKey Blockchain = "About_Blockchain";
            public static TranslationKey ContactUs = "About_ContactUs";
            public static TranslationKey Versions = "About_Versions";
            public static TranslationKey AppVersion = "About_AppVersion";
            public static TranslationKey BuildVersion = "About_BuildVersion";
            public static TranslationKey EthereumNetwork = "About_EthereumNetwork";
            public static TranslationKey ApiEnvironment = "About_ApiEnvironment";
        }

        public static class Backend
        {
            public static TranslationKey BuyingDone = "Notification_BuyingDone";
            public static TranslationKey PresaleOpen = "Notification_PresaleOpen";
            public static TranslationKey PresaleWillCloseSoon = "Notification_PresaleWillCloseSoon";
        }

        public static class Wallet
        {
            public static TranslationKey WithdrawEther = "Wallet_WithdrawEther";
            public static TranslationKey NoAddress = "Wallet_NoAddress";
            public static TranslationKey Help = "Wallet_Help";
            public static TranslationKey Balance = "Wallet_Balance";
            public static TranslationKey WalletAddress = "Wallet_Address";
            public static TranslationKey WhyNoAddress = "Wallet_WhyNoAddress"; 
            public static TranslationKey ReachTeam = "Wallet_ReachTeam";
            public static TranslationKey WithdrawQuestion = "Wallet_WithdrawQuestion";
            public static TranslationKey SendingTransaction = "Wallet_SendingTransaction";
            public static TranslationKey AlertTransactionError = "Wallet_AlertTransactionError";
            public static TranslationKey SuccessfullySent = "Wallet_SuccessfullySent";
        }

        public static class Documents
        {
            public static TranslationKey WhitePaper = "Document_WhitePaper";
            public static TranslationKey TermsAndServices = "Document_TermsAndServices";
            public static TranslationKey PrivacyPolicy = "Document_PrivacyPolicy";
        }
    }
}