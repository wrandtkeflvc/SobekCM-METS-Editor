﻿#region Using directives

using System;
using System.Drawing;
using SobekCM.METS_Editor.Template;
using SobekCM.Resource_Object;
using SobekCM.Resource_Object.Metadata_Modules;

#endregion

namespace SobekCM.METS_Editor.Elements
{
    /// <summary> Object used in the metadata template to display and allow the user 
    /// to edit the COMMON OR VERNACULAR NAME associated with a zoological taxonomy </summary>
    /// <remarks>This class extends the <see cref="simpleTextBox_Element"/> object.</remarks>
    public class ZT_Common_Name : simpleTextBox_Element
    {
        /// <summary> Constructor for a new ZT_Common_Name, used in the metadata
        /// template to display and allow the user to edit the common or vernacular 
        /// name associated with a zoological taxonomy </summary>
        public ZT_Common_Name() : base("Common Name")
        {
            // Set the type of this object
            base.type = Element_Type.ZT_CommonName;
            base.display_subtype = "simple";

            // Set some immutable characteristics
            always_single = true;
            always_mandatory = false;
            base.maximum_input_length = 350;
        }

        /// <summary> Returns the HTML URL Stub </summary>
        /// <returns></returns>
        public override string Help_URL()
        {
            return "zt_commonname";
        }

        /// <summary> Sets the language to use for the user interface on this element. </summary>
        /// <remarks> Sets the text for the label according to language </remarks>
        protected override void Inner_Set_Language(Template_Language newLanguage)
        {
            switch (newLanguage)
            {
                case Template_Language.English:
                    base.title = "Common Name";
                    break;
                case Template_Language.Spanish:
                    base.title = "Common Name";
                    break;
                case Template_Language.French:
                    base.title = "Common Name";
                    break;
                default:
                    base.title = "Common Name - unknown";
                    break;
            }
        }

        /// <summary> Set the minimum title length specific to the 
        /// implementation of abstract_Element.  </summary>
        /// <param name="size"> Height of the font </param>
        protected override void Inner_Set_Minimum_Title_Length(Font current_font, Template_Language current_language)
        {
            // Get the size of the font
            float font_size = 10.0F;

            font_size = Font.SizeInPoints;

            // Set the title length
            switch (current_language)
            {
                case Template_Language.English:
                    base.minimum_title_length = (int)(font_size * 12);
                    break;
                case Template_Language.Spanish:
                    base.minimum_title_length = (int)(font_size * 12);
                    break;
                case Template_Language.French:
                    base.minimum_title_length = (int)(font_size * 12);
                    break;
                default:
                    base.minimum_title_length = (int)(font_size * 12);
                    break;
            }
        }

        /// <summary> Prepares the bib object for the save, by clearing the 
        /// existing data in this element's related field. </summary>
        /// <param name="Bib"> Existing Bib object </param>
        public override void Prepare_For_Save(SobekCM_Item Bib)
        {
            Zoological_Taxonomy_Info darwinCoreInfo = Bib.Get_Metadata_Module(GlobalVar.ZOOLOGICAL_TAXONOMY_METADATA_MODULE_KEY) as Zoological_Taxonomy_Info;
            if (darwinCoreInfo != null)
            {
                darwinCoreInfo.Common_Name = String.Empty;
            }
        }

        /// <summary> Saves the data stored in this instance of the 
        /// element to the provided bibliographic object </summary>
        /// <param name="Bib"> Object into which to save this element's data </param>
        public override void Save_To_Bib(SobekCM_Item Bib)
        {
            if (base.thisBox.Text.Trim().Length > 0)
            {
                Zoological_Taxonomy_Info darwinCoreInfo = Bib.Get_Metadata_Module(GlobalVar.ZOOLOGICAL_TAXONOMY_METADATA_MODULE_KEY) as Zoological_Taxonomy_Info;
                if (darwinCoreInfo == null)
                {
                    darwinCoreInfo = new Zoological_Taxonomy_Info();
                    Bib.Add_Metadata_Module(GlobalVar.ZOOLOGICAL_TAXONOMY_METADATA_MODULE_KEY, darwinCoreInfo);
                }
                darwinCoreInfo.Common_Name = base.thisBox.Text.Trim();
            }
        }

        /// <summary> Saves the data stored in this instance of the 
        /// element to the provided bibliographic object </summary>
        /// <param name="Bib"> Object to populate this element from </param>
        public override void Populate_From_Bib(SobekCM_Item Bib)
        {
            Zoological_Taxonomy_Info darwinCoreInfo = Bib.Get_Metadata_Module(GlobalVar.ZOOLOGICAL_TAXONOMY_METADATA_MODULE_KEY) as Zoological_Taxonomy_Info;
            if (darwinCoreInfo != null)
            {
                base.thisBox.Text = darwinCoreInfo.Common_Name;
            }
        }
    }
}
