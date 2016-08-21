

namespace EAAddinFramework.Utils
{
	/// <summary>
	/// Description of User.
	/// </summary>
	public class User
	{
		public Model Model {get;set;}
		public string Login {get;set;}
		public string FirstName {get;set;}
		public string LastName {get;set;}
		public string FullName
		{
			get
			{
				if (Model.IsSecurityEnabled)
				{
					return FirstName + " " + LastName;
				}
				else
				{
					return Login;
				}
			}
		}
		
		/// <summary>
		/// creates a new user based on the given details
		/// </summary>
		/// <param name="model">the model containing the user</param>
		/// <param name="login">the string used to log in into the tool</param>
		/// <param name="firstName">the first name of the user</param>
		/// <param name="lastName">the last name of the user</param>
		public User(Model model,string login,string firstName,string lastName)
		{
			Model = model;
			Login = login;
			FirstName = firstName;
			LastName = lastName;
		}
	}
}
