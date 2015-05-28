using UnityEngine;
using System.Collections;


public class ShellUpgrade : MonoBehaviour {
	public string UpgradeName { get; set; }
	public virtual void OnAdd() { }
	public virtual void OnRemove() { }
}
