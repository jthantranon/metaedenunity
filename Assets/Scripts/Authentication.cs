using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Authentication : MonoBehaviour {
	private string authUrl = "http://127.0.0.1:8580/auth/";
	private string signupUrl = "http://127.0.0.1:8580/signup/";

	public GameObject loginPanel;
	public GameObject signUpPanel;

	public InputField authUsernameField;
	public InputField authPasswordField;

	public InputField signUpUsernameField;
	public InputField signUpPasswordField;
	public InputField signUpFirstNameField;
	public InputField signUpLastNameField;
	public InputField signUpEmailField;

	public void SwitchToSignup()
	{
		loginPanel.SetActive(false);
		signUpPanel.SetActive(true);
	}

	public void SwitchToLogin()
	{
		loginPanel.SetActive(true);
		signUpPanel.SetActive(false);
	}

	public void Authenticate()
	{
		var username = authUsernameField.text;
		var password = authPasswordField.text;
		Debug.Log(string.Format("username={0} password={1}", username, password));

		StartCoroutine(AuthenticateAsync(username, password));
	}

	public void SignUp()
	{
		var username = signUpUsernameField.text;
		var password = signUpPasswordField.text;
		var firstName = signUpFirstNameField.text;
		var lastName = signUpLastNameField.text;
		var email = signUpEmailField.text;
		Debug.Log(string.Format("username={0} password={1} firstName={2} lastName={3} email={4}", 
		                        username, password, firstName, lastName, email));
		
		StartCoroutine(SignUpAsync(username, password, firstName, lastName, email));
	}

	private IEnumerator AuthenticateAsync(string username, string password) {
		var form = new WWWForm();
		form.AddField("username", username);
		form.AddField("password", password);
		var www = new WWW(authUrl, form);
		Debug.Log("Executing");
		yield return www;
		if(www.error == null) {
			Debug.Log("Success: " + www.text);
			var pattern = "\"access_token\":\"(?<token>[A-Za-z0-9\\.=]+)\"";
			var match = System.Text.RegularExpressions.Regex.Match(www.text, pattern);
			var token = match.Groups["token"].Value;
			Debug.Log(token);
		} else {
			Debug.LogError("Error: " + www.error);
		}
	}

	private IEnumerator SignUpAsync(string username, string password, string firstName, string lastName, string email)
	{
		var form = new WWWForm();
		form.AddField("username", username);
		form.AddField("password", password);
		form.AddField("firstName", firstName);
		form.AddField("lastName", lastName);
		form.AddField("email", email);
		var www = new WWW(signupUrl, form);
		Debug.Log("Executing");
		yield return www;
		if(www.error == null) {
			Debug.Log("Success: " + www.text);
		} else {
			Debug.LogError("Error: " + www.error);
		}
	}
}
