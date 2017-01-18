﻿
using System.IO;
using System.Net;
using System.Web;
using System.Xml;

namespace TranslationRobot
{
    public class LocationInfo
    {

        private XmlDocument _ipInfoXML;
        public LocationInfo(string ipAddress)
        {
            string url = "http://ip-api.com/xml/";
            string checkURL = url + ipAddress;
            string ipResponse = RequestHelper.DownloadString(checkURL);

            _ipInfoXML = new XmlDocument();
            _ipInfoXML.LoadXml(ipResponse);

        }


        public string GetCountryName()
        {
            XmlNodeList responseXML = _ipInfoXML.GetElementsByTagName("country");
            return responseXML.Item(0).InnerText;
        }


        public string GetCountryCode()
        {
            XmlNodeList responseXML = _ipInfoXML.GetElementsByTagName("countryCode");
            return responseXML.Item(0).InnerText;
        }

        public static string GetLocationInfo(string address, TranslatorAccess translatorAccess)
        {
            string encodedAddress = HttpUtility.UrlEncode(address);
            string url = " http://dev.virtualearth.net/REST/v1/Locations/" + encodedAddress + "?o=xml&incl=queryParse,ciso2&inclnb=1&key=Ag2p3wHuXnmlaO-LffokUlisExjYT6n70Vo9t71n72V5qQ_ZqA6gmPT4cXuD1Ych";
            string result = RequestHelper.DownloadString(url);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(result);
            XmlNode resourceSet = xmlDocument.GetElementsByTagName("ResourceSet")[0];
            int numberOfResults = int.Parse(xmlDocument.GetElementsByTagName("EstimatedTotal").Item(0).InnerText);
            if (numberOfResults == 0)
            {
                throw new AddressNotFoundException("Address could not be located: " + address);
            }
            string country = xmlDocument.GetElementsByTagName("CountryRegionIso2").Item(0).InnerText;
            string languageCode = translatorAccess.GetLanguageCode(country);
            string localizedResult = RequestHelper.DownloadString(url + "&c=" + languageCode);
            XmlDocument localizedXmlDocument = new XmlDocument();
            localizedXmlDocument.LoadXml(localizedResult);
            string localizedAddress = localizedXmlDocument.GetElementsByTagName("FormattedAddress").Item(0).InnerText;
            return localizedAddress;
        }
    } 
}
