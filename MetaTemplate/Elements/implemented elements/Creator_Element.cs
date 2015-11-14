#region Using directives

using System;
using System.Drawing;
using SobekCM.METS_Editor.Template;
using SobekCM.Resource_Object;
using SobekCM.Resource_Object.Bib_Info;

#endregion

namespace SobekCM.METS_Editor.Elements
{
	/// <summary> Object used in the metadata template to display and allow the user 
	/// to edit the creator's name</summary>
	/// <remarks>This class extends the <see cref="simpleTextBox_Element"/> object.<br /><br />
	/// Written by Mark Sullivan ( 2006 ).</remarks>
	public class Creator_Simple_Element : simpleTextBox_Element
	{
        private string nameid;
        private bool contributor_exists;

		/// <summary> Constructor for a new Creator_Simple_Element, used in the metadata
		/// template to display and allow the user to edit the creator's name </summary>
		public Creator_Simple_Element( ) : base( "Creator" )
		{
			// Set the type of this object
			base.type = Element_Type.Creator;
			base.display_subtype = "simple";

			// Set some immutable characteristics
			always_single = false;
			always_mandatory = false;
            contributor_exists = false;
            nameid = String.Empty;
		}

        /// <summary> Flag can be set to indicate that creators with role = CONTRIBUTOR should be excluded  </summary>
        public bool Contributor_Exists
        {
            set
            {
                contributor_exists = value;
            }
            get
            {
                return contributor_exists;
            }
        }

        /// <summary> Returns the HTML URL Stub </summary>
        /// <returns></returns>
        public override string Help_URL()
        {
            return "creator_simple";
        }

		/// <summary> Sets the language to use for the user interface on this element. </summary>
		/// <remarks> Sets the text for the label according to language </remarks>
		protected override void Inner_Set_Language( Template_Language newLanguage )
		{
			switch( newLanguage )
			{
				case Template_Language.English:
					base.title = "Creator";
					break;
				case Template_Language.Spanish:
					base.title = "Autor o Creador";
					break;
				case Template_Language.French:
					base.title = "Cr�ateur";
					break;
				default:
					base.title = "Creator - unknown";
					break;
			}
		}

		/// <summary> Set the minimum title length specific to the 
		/// implementation of abstract_Element.  </summary>
		/// <param name="size"> Height of the font </param>
		protected override void Inner_Set_Minimum_Title_Length( Font current_font, Template_Language current_language )
		{
			// Get the size of the font
			float font_size = 10.0F;

				font_size = Font.SizeInPoints;

			// Set the title length
			switch( current_language )
			{
				case Template_Language.English:
					base.minimum_title_length = (int) (font_size * 6);
					break;
				case Template_Language.Spanish:
					base.minimum_title_length = (int) (font_size * 11);
					break;
				case Template_Language.French:
					base.minimum_title_length = (int) (font_size * 7);
					break;
				default:
					base.minimum_title_length = (int) (font_size * 10);
					break;
			}
		}

		/// <summary> Prepares the bib object for the save, by clearing the 
		/// existing data in this element's related field. </summary>
		/// <param name="Bib"> Existing Bib object </param>
		public override void Prepare_For_Save( SobekCM_Item Bib )
		{
            Bib.Bib_Info.Main_Entity_Name.Clear();
            Bib.Bib_Info.Clear_Names();
		}

		/// <summary> Saves the data stored in this instance of the 
		/// element to the provided bibliographic object </summary>
		/// <param name="Bib"> Object into which to save this element's data </param>
		public override void Save_To_Bib( SobekCM_Item Bib )
		{
			if ( base.thisBox.Text.Trim().Length > 0 )
			{
                Name_Info name = new Name_Info(thisBox.Text, String.Empty);
                name.ID = nameid;
                // Just set to personal so it goes to the 700 field and is read back in.
                name.Name_Type = Name_Info_Type_Enum.personal;
                Bib.Bib_Info.Add_Named_Entity(name);              
			}
		}

        /// <summary> Clones this element, not copying the actual data
        /// in the fields, but all other values. </summary>
        /// <returns>Clone of this element</returns>
        public override abstract_Element Clone()
        {
            // Get the new element
            Creator_Simple_Element newElement = (Creator_Simple_Element)Element_Factory.getElement(Type, Display_SubType);
            newElement.Location = Location;
            newElement.Language = Language;
            newElement.Title_Length = Title_Length;
            newElement.Lines = Lines;
            newElement.Height = Height;
            newElement.Font = Font;
            newElement.Set_Width(Width);
            newElement.Index = Index + 1;
            newElement.Contributor_Exists = contributor_exists;
            return newElement;
        }

		/// <summary> Saves the data stored in this instance of the 
		/// element to the provided bibliographic object </summary>
		/// <param name="Bib"> Object to populate this element from </param>
		public override void Populate_From_Bib( SobekCM_Item Bib )
		{
            if (!contributor_exists)
            {
                if (Bib.Bib_Info.Main_Entity_Name.hasData)
                {
                    if (base.index == 0)
                    {
                        base.thisBox.Text = Bib.Bib_Info.Main_Entity_Name.Full_Name;
                        nameid = Bib.Bib_Info.Main_Entity_Name.ID;
                    }
                    else
                    {
                        if (base.index <= Bib.Bib_Info.Names.Count)
                        {
                            base.thisBox.Text = Bib.Bib_Info.Names[base.index - 1].Full_Name;
                            nameid = Bib.Bib_Info.Names[base.index - 1].ID;
                        }
                    }
                }
                else
                {
                    if (base.index < Bib.Bib_Info.Names.Count)
                    {
                        base.thisBox.Text = Bib.Bib_Info.Names[base.index].Full_Name;
                        nameid = Bib.Bib_Info.Names[base.index].ID;
                    }
                }
            }
            else
            {
                if ((Bib.Bib_Info.Main_Entity_Name.hasData) && ((Bib.Bib_Info.Main_Entity_Name.Roles.Count == 0) || (Bib.Bib_Info.Main_Entity_Name.Roles[0].Role.ToUpper() != "CONTRIBUTOR") || ( Bib.Bib_Info.Main_Entity_Name.Roles[0].Role.ToUpper() == "CTB" )))
                {
                    if (base.index == 0)
                    {
                        base.thisBox.Text = Bib.Bib_Info.Main_Entity_Name.Full_Name;
                        nameid = Bib.Bib_Info.Main_Entity_Name.ID;
                    }
                    else
                    {
                        if (base.index <= Bib.Bib_Info.Names.Count)
                        {
                            int noncontributor_count = 0;
                            foreach (Name_Info thisName in Bib.Bib_Info.Names)
                            {
                                if ((thisName.Roles.Count == 0) || ((thisName.Roles[0].Role.ToUpper() != "CONTRIBUTOR") && ( thisName.Roles[0].Role.ToUpper() != "CTB" )))
                                {
                                    noncontributor_count++;
                                    if (noncontributor_count == base.index)
                                    {
                                        base.thisBox.Text = thisName.Full_Name;
                                        nameid = thisName.ID;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    int noncontributor_count = -1;
                    foreach (Name_Info thisName in Bib.Bib_Info.Names)
                    {
                        if ((thisName.Roles.Count == 0) || ((thisName.Roles[0].Role.ToUpper() != "CONTRIBUTOR") && (thisName.Roles[0].Role.ToUpper() != "CTB")))
                        {
                            noncontributor_count++;
                            if (noncontributor_count == base.index)
                            {
                                base.thisBox.Text = thisName.Full_Name;
                                nameid = thisName.ID;
                                break;
                            }
                        }
                    }
                }
            }
		}
	}
}