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
        public ActionResult Index(int scenarioID, int panelCount)
        {
            Creator creator = new Creator();
            creator.scenarioID = scenarioID;
            creator.scenarioName = GetScenarioName(scenarioID);
            creator.htmlDefaultPanel = GetDefaultPanel(scenarioID, (panelCount > 4) ? 4 : panelCount);
            return View(creator);
        }

        [ActionName("PanelCount")]
        public ActionResult SelectPanel(int scenarioID)
        {
            Creator creator = new Creator();
            creator.scenarioID = scenarioID;
            creator.scenarioName = GetScenarioName(scenarioID);
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
        public int GetScenarioPeopleImageContextCount(int scenarioID)
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
        public int GetScenarioDialogImageContextCount(int scenarioID)
        {
            int imgCount = 0;
            switch (scenarioID)
            {
                case 1:
                    string[] scenarioImageList = { "../Content/img/leftdialog.png", 
                                                   "../Content/img/midleftdialog.png",
                                                   "../Content/img/middialog.png",
                                                   "../Content/img/rightdialog.png",
                                                   "../Content/img/midrightdialog.png"
                                                 };
                    imgCount = scenarioImageList.GetLength(0);
                    break;
            }
            return imgCount;
        }

        [NonAction]
        public string GetScenarioPeopleImageContextUrl(int scenarioID, int index)
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

        [NonAction]
        public string GetPropValueForDialog(int categoryID, int index, int panelIndex = 0)
        {
            string result = string.Empty;
            switch (categoryID)
            {
                case 1: // img url
                    string[] scenarioImageContextList = { "../Content/img/leftdialog.png", 
                                                          "../Content/img/midleftdialog.png",
                                                          "../Content/img/middialog.png",
                                                          "../Content/img/rightdialog.png",
                                                          "../Content/img/midrightdialog.png" 
                                                        };
                    result = scenarioImageContextList[index - 1];
                    break;

                case 2: // img dynamic id, arrangement based on case 1
                    string[] scenarioDynamicId = { "leftdialog", 
                                                   "midleftdialog",
                                                   "middialog",
                                                   "rightdialog",
                                                   "midrightdialog" 
                                                 };
                    result = scenarioDynamicId[index - 1] + "_" + panelIndex.ToString().Trim() + "_1"; // _1 kasi unang add palang
                    break;
            }
            return result;
        }

        #region "Dynamic Creation of Controls"
        
        [NonAction]
        public string GetDefaultPanel(int scenarioID, int panelCount)
        {
            // default panel count = 2

            StringWriter stringWriter = new StringWriter();

            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                int rowCount = 1;
                if (panelCount > 2)
                {
                    rowCount = 2;
                }

                int p = 1;
                for (int r = 1; r <= rowCount; r++)
                {
                    // row main
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, "pnlsrow_div" + r);
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    
                    for (int l = 1; l <= 2; l++)
                    {
                        // panel 
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "col pnl");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "pnl" + p);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        // div row
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "row justify-content-center");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        // div holder 
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "dmDivHolder col-11");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "dh" + p);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        // image background 
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "pnlimage");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "imgpnlback" + p);
                        writer.AddAttribute(HtmlTextWriterAttribute.Src, GetScenarioUrlImage(scenarioID, 1));
                        writer.RenderBeginTag(HtmlTextWriterTag.Img);

                        writer.RenderEndTag(); // end for image background 
                        writer.RenderEndTag(); // end for div holder 
                        writer.RenderEndTag(); // end for div row 

                        // for buttons :: Add People, Add Dialog and Change Background

                        // row for btns
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "row justify-content-center");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "rowforbtn" + p);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        // col-11 for btns
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "col-11");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);


                        #region "add people button"

                        // main div for buttons below
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "row");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);


                        // add people
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "col-4 popup btn-dark btn-cust-style");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "PopupMenu(this)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "btnadd_p_" + p);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        writer.Write("Add People");

                        // add people : popup
                        // row
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "row popuptext");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "popupmenu_p_row" + p);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        // get total count of image
                        int TotalImageCount = GetScenarioPeopleImageContextCount(scenarioID);

                        for (int i = 1; i <= TotalImageCount; i++)
                        {
                            // div
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "col");
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "AddPeople(this)");
                            writer.AddAttribute(HtmlTextWriterAttribute.Id, "popupmenu_p_div_" + p.ToString().Trim() + "_" + i.ToString().Trim());
                            writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);

                            // image
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "imgContext");
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "AddPeople(this)");
                            writer.AddAttribute(HtmlTextWriterAttribute.Id, "cimg_p_" + p.ToString().Trim() + "_" + i.ToString().Trim());
                            writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                            writer.AddAttribute(HtmlTextWriterAttribute.Src, GetScenarioPeopleImageContextUrl(scenarioID, i));
                            writer.RenderBeginTag(HtmlTextWriterTag.Img);

                            writer.RenderEndTag(); // end for image
                            writer.RenderEndTag(); // end for div
                        }

                        writer.RenderEndTag(); // end for row
                        writer.RenderEndTag(); // end for add people div

                        #endregion

                        #region "add dialog button"

                        // add dialog
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "col-4 popup btn-dark btn-cust-style");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "PopupMenu(this)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "btnadd_d_" + p);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        writer.Write("Add Dialog");

                        // add dialog : popup
                        // row
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "row popuptext");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "popupmenu_d_row" + p);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        // get total count of image
                        TotalImageCount = GetScenarioDialogImageContextCount(scenarioID);

                        for (int i = 1; i <= TotalImageCount; i++)
                        {
                            // div
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "col");
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "AddDialog(this)");
                            writer.AddAttribute(HtmlTextWriterAttribute.Id, "popupmenu_d_div" + i.ToString().Trim());
                            writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);

                            // image
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "imgContext");
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "AddDialog(this)");
                            writer.AddAttribute(HtmlTextWriterAttribute.Id, GetPropValueForDialog(2, i, p));
                            writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                            writer.AddAttribute(HtmlTextWriterAttribute.Src, GetPropValueForDialog(scenarioID, i));
                            writer.RenderBeginTag(HtmlTextWriterTag.Img);

                            writer.RenderEndTag(); // end for image
                            writer.RenderEndTag(); // end for div
                        }

                        writer.RenderEndTag(); // end for row
                        writer.RenderEndTag(); // end for add dialog div

                        #endregion

                        #region "change background button"

                        // add dialog
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "col-4 popup btn-dark btn-cust-style");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "PopupMenu(this)");
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "btnchng_bck_" + p);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        writer.Write("Change Background");

                        // add dialog : popup
                        // row
                        writer.AddAttribute(HtmlTextWriterAttribute.Class, "row popuptext");
                        writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                        writer.AddAttribute(HtmlTextWriterAttribute.Id, "popupmenu_chngbck_row" + p);
                        writer.RenderBeginTag(HtmlTextWriterTag.Div);

                        // get total count of image
                        TotalImageCount = GetScenarioDialogImageContextCount(scenarioID);

                        for (int i = 1; i <= TotalImageCount; i++)
                        {
                            // div
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "col");
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "AddDialog(this)");
                            writer.AddAttribute(HtmlTextWriterAttribute.Id, "popupmenu_chngbck_div" + i.ToString().Trim());
                            writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                            writer.RenderBeginTag(HtmlTextWriterTag.Div);

                            // image
                            writer.AddAttribute(HtmlTextWriterAttribute.Class, "imgContext");
                            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "AddDialog(this)");
                            writer.AddAttribute(HtmlTextWriterAttribute.Id, "cimg_chngbck_" + i.ToString().Trim());
                            writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                            writer.AddAttribute(HtmlTextWriterAttribute.Src, GetPropValueForDialog(scenarioID, i));
                            writer.RenderBeginTag(HtmlTextWriterTag.Img);

                            writer.RenderEndTag(); // end for image
                            writer.RenderEndTag(); // end for div
                        }

                        writer.RenderEndTag(); // end for row
                        writer.RenderEndTag(); // end for add dialog div

                        #endregion

                        writer.RenderEndTag(); // end for col-11 for btns
                        writer.RenderEndTag(); // end for row for btns
                        writer.RenderEndTag(); // end for ?????
                        writer.RenderEndTag(); // end for panel 

                        p += 1;

                        if (p > panelCount)
                        {
                            if (panelCount == 1 || panelCount == 3)
                            { 
                                // panel
                                writer.AddAttribute(HtmlTextWriterAttribute.Class, "col");
                                writer.AddAttribute(HtmlTextWriterAttribute.Name, p.ToString().Trim());
                                writer.AddAttribute(HtmlTextWriterAttribute.Id, "pnl" + p);
                                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                                writer.RenderEndTag(); // end for panel 
                            }

                            break;
                        }
                    }

                    writer.RenderEndTag(); // end for row main

                    if ((r + 1) <= rowCount)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Br);
                        writer.RenderBeginTag(HtmlTextWriterTag.Br);
                    }   
                }
            }

            return stringWriter.ToString();
        }





        #endregion

    }
}
