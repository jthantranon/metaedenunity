using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Authentication : MonoBehaviour {
	public static string Token;

	private string authUrl = "http://127.0.0.1:8580/auth/";
	private string signupUrl = "http://127.0.0.1:8580/signup/";

	public GameObject loginPanel;
	public GameObject signUpPanel;

	public InputField authUsernameField;
	public InputField authPasswordField;
	public Text authError;

	public InputField signUpUsernameField;
	public InputField signUpPasswordField;
	public InputField signUpFirstNameField;
	public InputField signUpLastNameField;
	public InputField signUpEmailField;

	void Start()
	{
		authError.gameObject.SetActive(false);
	}

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
			var settings2 = new Pathfinding.Serialization.JsonFx.JsonReaderSettings();
			var reader = new Pathfinding.Serialization.JsonFx.JsonReader(www.text, settings2);
			var resultObject = reader.Deserialize() as IDictionary<string, object>;
			Token = resultObject["access_token"] as string;
			Application.LoadLevel("FirstScene");
			Debug.Log(Token);
			authError.gameObject.SetActive(false);
		} else {
			authError.gameObject.SetActive(true);
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
			StartCoroutine(AuthenticateAsync(username, password));
		} else {
			Debug.LogError("Error: " + www.error);
		}
	}
}
