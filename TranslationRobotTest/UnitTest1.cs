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
        public void TestGetLocationInfo()
        {
            var translatorAccess = new TranslatorAccess();
            string result = LocationInfo.GetLocationInfo("Cologne", translatorAccess);
            Assert.AreEqual("Köln, NW, Deutschland", result);

            result = LocationInfo.GetLocationInfo("Bonner Str 17, Cologne", translatorAccess);
            Assert.AreEqual("Bonner Straße 17, 50677 Köln, Deutschland", result);

            result = LocationInfo.GetLocationInfo("Bonner, Str, 17, Cologne", translatorAccess);
            Assert.AreEqual("Bonner Straße 17, 50677 Köln, Deutschland", result);

            result = LocationInfo.GetLocationInfo("1 Queen's Road Central, Hongkong", translatorAccess);
            Assert.AreEqual("香港中環皇后大道中1號", result);

            result = LocationInfo.GetLocationInfo("Guangzhou", translatorAccess);
            Assert.AreEqual("广东省广州市", result);
           




        }

        [TestMethod]
        public void TestGoogle()
        {
            var translatorAccess = new TranslatorAccess();
            string result = LocationInfo.GetLocalizedAddressFromGoogle("Hasenhaus 2, Haan", translatorAccess);
            Assert.AreEqual("Hasenhaus 2, 42781 Haan, Deutschland", result);


            result = LocationInfo.GetLocalizedAddressFromGoogle("1 Queen's Road Central, Hongkong", translatorAccess);
            Assert.AreEqual("香港中環皇后大道中1號匯豐總行大廈", result);

            result = LocationInfo.GetLocalizedAddressFromGoogle("Guangzhou", translatorAccess);
            Assert.AreEqual("中国广东省广州市", result);



            result = LocationInfo.GetLocalizedAddressFromGoogle("Platz des himmlischen Friedens", translatorAccess);
            Assert.AreEqual("中国北京市东城区", result);

            result = LocationInfo.GetLocalizedAddressFromGoogle("25B Kalyani Nagar, Pune", translatorAccess);
            Assert.AreEqual("कल्याणी नगर, पुणे, महाराष्ट्र, भारत", result);

            result = LocationInfo.GetLocalizedAddressFromGoogle("The Westin, Pune", translatorAccess);
            Assert.AreEqual("कल्याणी नगर, पुणे, महाराष्ट्र, भारत", result);
            

        }

    }
}
