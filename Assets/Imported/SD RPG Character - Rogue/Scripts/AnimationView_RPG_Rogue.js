private var anim : Animation;

private var loop  : String[]=["rogue01_idle@loop", "rogue01_run@loop", "rogue01_stun@loop"];
private var damage : String[]=["rogue01_damage","rogue01_die"];
private var attack   : String[]=["rogue01_attack_base", "rogue01_attack_skill01"];
private var etc  : String[]=["rogue01_jump", "rogue01_sit", "rogue01_sit_idle@loop", "rogue01_stand"];

function Start () {
	anim=GetComponent (Animation);

	anim["rogue01_idle@loop"].speed=1.0;
}

function OnGUI () {

	CategoryView ("loop", loop, 10);
	CategoryView ("damage", damage, 170);
	CategoryView ("attack", attack, 330);
	CategoryView ("etc", etc, 490);
}

function CategoryView (nme:String, cat:String[], x:int) {
	GUI.Box (Rect(x,10,150,23), nme);
	for (var i:int=0; i<cat.Length; i++) {
		if (GUI.Button (Rect(x, 35+(25*i), 150, 23), cat[i]) ) {
			
			GoAnim (cat[i]);
		}
	}

}

function GoAnim (nme:String) {

	anim.CrossFade (nme);

	while (anim.IsPlaying) {
		yield;
	}

	anim.CrossFade ("rogue01_idle@loop");
}