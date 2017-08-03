using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VECTO_Engine;

namespace VectoEngineTest
{
	[TestClass]
	public class UnitTest1
	{
		private const string basepath =
			@"TestData";

		[TestInitialize]
		public void SetCulture()
		{
			try {
				Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			}
			catch (Exception ex) {
				Assert.Fail();
			}
		}

		[TestMethod]
		public void TestMethod1()
		{
			var Job = new cJob();

			var jobDir = "Valid_1.4";

			Job.Manufacturer = "TUG";
			Job.Model = "Testengine";
			Job.CertNumber = "Engine0815";
			Job.Idle_Parent = 600;
			Job.Idle = 600;
			Job.Displacement = 7700;
			Job.RatedPower = 130;
			Job.RatedSpeed = 2200;
			Job.FuelType = "Ethanol / CI";
			Job.NCVfuel = 42.3;

			Job.MapFile = Path.Combine(basepath, jobDir, "Map.csv");
			Job.FlcFile = Path.Combine(basepath, jobDir, "FullLoad_Child.csv");
			Job.FlcParentFile = Path.Combine(basepath, jobDir, "FullLoad_Parent.csv");
			Job.DragFile = Path.Combine(basepath, jobDir, "Motoring.csv");

			Job.FCspecMeas_ColdTot = 200;
			Job.FCspecMeas_HotTot = 200;
			Job.FCspecMeas_HotUrb = 200;
			Job.FCspecMeas_HotRur = 200;
			Job.FCspecMeas_HotMw = 200;
			Job.CF_RegPer = 1;

			Job.OutPath = CreateOutputDirectory(jobDir);


			var worker = new BackgroundWorker();
			worker.WorkerReportsProgress = true;
			worker.ProgressChanged += ProgressChangedEventHandler;
			GlobalDefinitions.Worker = worker;

			var success = Job.Run();

			Assert.IsTrue(success);

			var componentFile = Path.Combine(Job.OutPath, string.Format("results{0}_{1}.xml", Job.Manufacturer, Job.Model));
			var xml = XDocument.Load(new XmlTextReader(componentFile));

			Assert.AreEqual(1.0, GetDoubleValue(xml, "WHTCUrban"), 1e-4);
			Assert.AreEqual(1.0741, GetDoubleValue(xml, "WHTCRural"), 1e-4);
			Assert.AreEqual(1.1237, GetDoubleValue(xml, "WHTCMotorway"), 1e-4);
			Assert.AreEqual(1.0, GetDoubleValue(xml, "BFColdHot"), 1e-4);
			Assert.AreEqual(1.1, GetDoubleValue(xml, "CFRegPer"), 1e-4);
			Assert.AreEqual(1.6459, GetDoubleValue(xml, "CFNCV"), 1e-4);
		}

		private double GetDoubleValue(XDocument xml, string elementName)
		{
			var xpath = string.Format("//*[local-name()='{0}']", elementName);
			var node = xml.XPathSelectElement(xpath);
			return XmlConvert.ToDouble(node.Value);
		}


		private static string CreateOutputDirectory(string jobDir)
		{
			var outPath = Path.Combine(basepath, jobDir, "results");
			if (Directory.Exists(outPath)) {
				Directory.Delete(outPath, true);
			}
			Directory.CreateDirectory(outPath);
			return outPath + Path.DirectorySeparatorChar;
		}

		public static void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs progressChangedEventArgs)
		{
			var msg = progressChangedEventArgs.UserState as GlobalDefinitions.cWorkerMsg;
			if (msg == null) {
				return;
			}
			Console.WriteLine(msg.MsgType + " - " + msg.Msg);
		}
	}
}