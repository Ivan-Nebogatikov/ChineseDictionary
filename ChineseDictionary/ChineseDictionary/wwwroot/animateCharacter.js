function drawCharacter(character, color) {
	var writer = HanziWriter.create('character-target-div', character, {
		width: 100,
		height: 100,
		padding: 5,
		strokeColor: color
	});
	document.getElementById('animate-button').addEventListener('click', function () {
		writer.animateCharacter();
	});
}

function removeListeners() {
	document.getElementById('character-target-div').innerHTML = "";
}