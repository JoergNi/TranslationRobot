using Microsoft.VisualStudio.TestTools.UnitTesting;
using TranslationRobot;

namespace TranslationRobotTest
{
    [TestClass]
    public class TranslatorAccessTest
    {
        [TestMethod]
        public void TestTranslate()
        {
            var translatorAccess = new TranslatorAccess();
            string result = translatorAccess.TranslateByCountryCode("Guten Morgen", "us");
            Assert.AreEqual("Good morning", result);
            result = translatorAccess.TranslateByCountryCode("Guten Morgen", "gb");
            Assert.AreEqual("Good morning", result);
            result = translatorAccess.TranslateByCountryCode("Guten Morgen", "au");
            Assert.AreEqual("Good morning", result);
            Assert.AreEqual("Good morning", result);
            result = translatorAccess.TranslateByCountryCode("Guten Morgen", "de");
            Assert.AreEqual("Guten Morgen", result);
            result = translatorAccess.TranslateByCountryCode("Guten Morgen", "co");
            Assert.AreEqual("Buenos días", result);
            result = translatorAccess.TranslateByCountryCode("Guten Morgen", "hk");
            Assert.AreEqual("早上好", result);




        }

        [TestMethod]
        public void TestGetLanguageCode()
        {
            var translatorAccess = new TranslatorAccess();
            string result = translatorAccess.GetLanguageCode("de");
            Assert.AreEqual("de", result);
            result = translatorAccess.GetLanguageCode("us");
            Assert.AreEqual("en", result);
            result = translatorAccess.GetLanguageCode("at");
            Assert.AreEqual("de", result);

        }


       
        [TestMethod]
        public void TestGoogle()
        {
            var translatorAccess = new TranslatorAccess();
            TranslatedAddressEntity result = LocationInfo.GetLocationFromGoogle("Hasenhaus 2, Haan", translatorAccess);
            Assert.AreEqual("Hasenhaus 2, 42781 Haan, Deutschland", result.Translation);


            result = LocationInfo.GetLocationFromGoogle("1 Queen's Road Central, Hongkong", translatorAccess);
            Assert.AreEqual("香港中環皇后大道中1號匯豐總行大廈", result.Translation);

            result = LocationInfo.GetLocationFromGoogle("Guangzhou", translatorAccess);
            Assert.AreEqual("中国广东省广州市", result.Translation);



            result = LocationInfo.GetLocationFromGoogle("Platz des himmlischen Friedens", translatorAccess);
            Assert.AreEqual("中国北京市东城区", result.Translation);

            result = LocationInfo.GetLocationFromGoogle("25B Kalyani Nagar, Pune", translatorAccess);
            Assert.AreEqual("Central Avenue Road & Shastri Nagar Road, गुड विल एन्क्लेव, शास्त्री नगर, पुणे 411006, भारत", result.Translation);

            result = LocationInfo.GetLocationFromGoogle("The Westin, Pune", translatorAccess);
            Assert.AreEqual("गणेश बाग, कोरेगांव, पुणे 411036, भारत", result.Translation);
            

        }

    }
}
