using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using ComicCreator.Models;
using System.Text;
using System.Web.UI;
using System.IO;
using HtmlAgilityPack;

namespace ComicCreator.Controllers
{
    public class CreatorController : Controller
    {
        
        public ActionResult Index(int scenarioID)
        {
            Creator creator = new Creator();
            creator.scenarioID = scenarioID;
            creator.scenarioName = GetScenarioName(scenarioID);
            creator.htmlDefaultPanel = GetDefaultPanel(scenarioID);
            return View(creator);
        }

        [NonAction]
        public string GetScenarioName(int scenarioID)
        {
            string[] scenarioName = { "School Scenario", "Garden Scenario", "Mall Scenario" };
            return scenarioName[scenarioID - 1];
        }

        [NonAction]
        public string GetScenarioUrlImage(int scenarioID, int imgID)
        {
            string scenarioImage = string.Empty;
            switch (scenarioID)
            {
                case 1:
                    string[] scenarioImageSource = { "../Content/img/scene1.jpg", "../Content/img/scene2.jpg" };
                    scenarioImage = scenarioImageSource[imgID - 1];
                    break;
            }
            return scenarioImage;
        }

        [NonAction]
        public int GetScenarioImageContextCount(int scenarioID)
        {
            int imgCount = 0;
            switch (scenarioID)
            {
                case 1:
                    string[] scenarioImageList = { "../Content/img/imgpeople1.png", "../Content/img/imgpeople2.png" };
                    imgCount = scenarioImageList.GetLength(0);
                    break;
            }
            return imgCount;
        }

        [NonAction]
        public string GetScenarioImageContextUrl(int scenarioID, int index)
        {
            string imgUrl = string.Empty;
            switch (scenarioID)
            {
                case 1:
                    string[] scenarioImageContextList = { "../Content/img/imgpeople1.png", "../Content/img/imgpeople2.png" };
                    imgUrl = scenarioImageContextList[index - 1];
                    break;
            }
            return imgUrl;
        }

        #region "Dynamic Creation of Controls"
        
        [NonAction]
        public string GetDefaultPanel(int scenarioID)
        {
            // default panel count = 2

            StringWriter stringWriter = new StringWriter();

            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                var panelCount = 1;

                // row main
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "pnlsrow" + panelCount);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                // panel 1
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "col pnl");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "pnl" + panelCount);
                writer.RenderBeginTag(HtmlTextWriterTag.Div); 

                // div holder 1
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "dmDivHolder");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "dh" + panelCount);
                writer.RenderBeginTag(HtmlTextWriterTag.Div); 

                // image background 1
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "pnlimage");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "imgpnlback" + scenarioID);
                writer.AddAttribute(HtmlTextWriterAttribute.Src, GetScenarioUrlImage(scenarioID, panelCount));
                writer.RenderBeginTag(HtmlTextWriterTag.Img);

                writer.RenderEndTag(); // end for image background 1
                writer.RenderEndTag(); // end for div holder 1

                // for buttons :: Add People, Add Dialog and Delete Panel

                // row for btns
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "rowforbtn" + panelCount);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                // add people
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "popup btn-dark");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "PopupMenu(this)");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "btnaddpeople" + panelCount);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.Write("Add People");

                // add people : popup
                // row
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "row popuptext");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "popupmenu" + panelCount);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                // get total count of image
                int TotalImageCount = GetScenarioImageContextCount(scenarioID);

                for (int i = 1; i <= TotalImageCount; i++)
                {
                    // div
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "col");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "AddImage(this)");
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    // image
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "imgContext");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "AddImage(this)");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "cimg" + i.ToString().Trim());
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, GetScenarioImageContextUrl(scenarioID, i));
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);

                    writer.RenderEndTag(); // end for image
                    writer.RenderEndTag(); // end for div
                }

                writer.RenderEndTag(); // end for row
                writer.RenderEndTag(); // end for add people div
                writer.RenderEndTag(); // end for row for btns
                writer.RenderEndTag(); // end for panel 1

                panelCount += 1;

                // panel 2
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "col pnl");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "pnl2");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                // div holder 2
                writer.AddAttribute(HtmlTextWriterAttribute.Style, "width: 100%; overflow: hidden;");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "dh2");
                writer.RenderBeginTag(HtmlTextWriterTag.Div); 

                // image background 2
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "pnlimage");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "imgpnlback2");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                writer.AddAttribute(HtmlTextWriterAttribute.Src, GetScenarioUrlImage(1, 2));
                writer.RenderBeginTag(HtmlTextWriterTag.Img);

                writer.RenderEndTag(); // end for image background 2
                writer.RenderEndTag(); // end for div holder 2

                // for buttons :: Add People, Add Dialog and Delete Panel
                // row for btns
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "rowforbtn" + panelCount);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                // add people
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "popup btn-dark");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "PopupMenu(this)");
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "btnaddpeople" + panelCount);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                writer.Write("Add People");

                // add people : popup
                // row
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "row popuptext");
                writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                writer.AddAttribute(HtmlTextWriterAttribute.Id, "popupmenu" + panelCount);
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                // get total count of image
                TotalImageCount = GetScenarioImageContextCount(scenarioID);

                for (int i = 1; i <= TotalImageCount; i++)
                {
                    // div
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "col");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "AddImage(this)");
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    // image
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "imgContext");
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "AddImage(this)");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "cimg" + i.ToString().Trim());
                    writer.AddAttribute(HtmlTextWriterAttribute.Name, panelCount.ToString().Trim());
                    writer.AddAttribute(HtmlTextWriterAttribute.Src, GetScenarioImageContextUrl(scenarioID, i));
                    writer.RenderBeginTag(HtmlTextWriterTag.Img);

                    writer.RenderEndTag(); // end for image
                    writer.RenderEndTag(); // end for div
                }

                writer.RenderEndTag(); // end for row
                writer.RenderEndTag(); // end for add people div
                writer.RenderEndTag(); // end for row for btns
                writer.RenderEndTag(); // end for panel 2

                writer.RenderEndTag(); // end for row main
            }

            return stringWriter.ToString();
        }





        #endregion

    }
}
