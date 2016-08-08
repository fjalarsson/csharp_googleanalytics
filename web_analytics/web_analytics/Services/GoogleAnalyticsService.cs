using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using web_analytics.Models;

namespace web_analytics.Services
{
	public class GoogleAnalyticsService
	{
		/// <summary>
		///  Retrieve data from google analytics. 
		/// <para></para>
		/// Parameters allow to set number of days back to get data from, how many results and specified queries.
		/// </summary>
		/// <param name="numberOfDays">Days back in time where counting shall start from</param>
		/// <param name="count">Number of results we want.</param>
		/// <param name="dimensions">If user specifies dimension</param>
		/// <param name="metrics">If user specifies metric</param>
		/// <returns>List of visits on all pages.</returns>
		public List<PageVisit> GetAnalyticsData(int numberOfDays, int count, string dimensions = "ga:date,ga:pagePath,ga:pageTitle", string metrics = "ga:visits")
		{
			//Specify where your p12 file is located. P12 keys are created in the Google Developer Console.		
			var keyFilePath = ConfigurationManager.AppSettings["keyFilePath"];

			//This is the private key's password that was generated along with the key.
			var keyPassword = ConfigurationManager.AppSettings["keyPassword"];

			//This is the email address from the Service Account credentials. 
			//If you did not write it down after the creation, you can access it anytime using the Google Developers Console
			var serviceAccountEmail = ConfigurationManager.AppSettings["serviceAccountEmail"];

			//This is the code of the website we have paired with the Reporting API. 
			//To get the code, the fastest way is to navigate to the settings of the desired website in Google Analytics and copy the eight digit code following the letter “p” from the URL of the browser. 
			var websiteCode = ConfigurationManager.AppSettings["websiteCode"];

			var certificate = new X509Certificate2(keyFilePath, keyPassword, X509KeyStorageFlags.Exportable);

			var scopes = new[]
			{
				AnalyticsService.Scope.AnalyticsReadonly
			};

			var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
			{
				Scopes = scopes
			}.FromCertificate(certificate));

			//This is the Analytics service that will be used for querying the statistics.
			var service = new AnalyticsService(new BaseClientService.Initializer
			{
				HttpClientInitializer = credential
			});

			//Google analytics data, for more metrics and dimensions: 
			//https://developers.google.com/analytics/devguides/reporting/core/dimsmets
			var request = service.Data.Ga.Get(
				"ga:" + websiteCode,
				DateTime.Now.AddDays(-numberOfDays).ToString("yyyy-MM-dd"),
				DateTime.Now.ToString("yyyy-MM-dd"),
				metrics);
			request.Dimensions = dimensions;
			var data = request.Execute();

			var entities = new List<PageVisit>();
			if (!string.IsNullOrWhiteSpace(dimensions))
			{
			var dimArray = dimensions.Split(',').Length;
			switch (dimArray)
			{
				case 1:
					entities.AddRange(data.Rows.Take(count).Select(row => new PageVisit
					{
						Dimension1 = row[0],
						Dimension2 = row[1]
					}));
					break;
				case 2:
					entities.AddRange(data.Rows.Take(count).Select(row => new PageVisit
					{
						Dimension1 = row[0],
						Dimension2 = row[1],
						Dimension3 = row[2]
					}));
					break;
				case 3:
					entities.AddRange(data.Rows.Take(count).Select(row => new PageVisit
					{
						Dimension1 = row[0],
						Dimension2 = row[1],
						Dimension3 = row[2],
						Dimension4 = row[3]

					}));
					break;
				default:
					entities.AddRange(data.Rows.Take(count).Select(row => new PageVisit
					{
						Dimension1 = row[0],
						Dimension2 = row[1],
						Dimension3 = row[2],
						Dimension4 = row[3],

					}));
					break;

			}

			return entities;
			}

			entities.AddRange(data.Rows.Take(count).Select(row => new PageVisit
			{
				Dimension1 = row[0]
			}));
			return entities;
		}
	}
}