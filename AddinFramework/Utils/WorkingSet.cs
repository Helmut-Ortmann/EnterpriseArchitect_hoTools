
using System;

namespace EAAddinFramework.Utils
{
	/// <summary>
	/// Description of WorkingSet.
	/// </summary>
	public class WorkingSet
	{
		public User User {get;set;}
		public string Name {get;set;}
		public Model Model {get;set;}
		public string Id {get;set;}
		
		public WorkingSet(Model model,string id,User user, string name)
		{
			Model = model;
			Id = id;
			User = user;
			Name = name;
		}
		/// <summary>
		/// copy the working set tot the given user
		/// </summary>
		/// <param name="user">the user to copy the working set to</param>
		/// <param name="overwrite">if true then the first working set found with the same name
		/// for the given user will be overwritten</param>
		public void CopyToUser(User user, bool overwrite)
		{
			if (overwrite)
			{
				//check if a working set with the same name already exists
				WorkingSet workingSetToOverwrite = Model.WorkingSets.Find(w => 
				                                                    w.User != null
				                                                    && w.User.Login == user.Login 
				                                                    && w.Name == Name);
			    workingSetToOverwrite?.Delete();
			}
			string insertQuery = @"insert into t_document (DocID,DocName, Notes, Style,ElementID, ElementType,StrContent,BinContent,DocType,Author,DocDate )
								select '"+Guid.NewGuid().ToString("B")+@"',d.DocName, d.Notes, d.Style,
								d.ElementID, d.ElementType,d.StrContent,d.BinContent,d.DocType,'" + user.FullName + @"',d.DocDate from t_document d
								where d.DocID like '"+Id+"'";
			Model.ExecuteSql(insertQuery);
		
		}
		/// <summary>
		/// deletes the working set from the database
		/// </summary>
		void Delete()
		{
			string deleteQuery = $"delete from t_document where docid like '{Id} '";
			Model.ExecuteSql(deleteQuery);
		}
	}
}
