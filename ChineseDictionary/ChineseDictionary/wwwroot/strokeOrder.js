function characterDraw() {
	var writer = HanziWriter.create('character-target-div', '国', {
		width: 100,
		height: 100,
		padding: 5,
		showOutline: true
	});
	document.getElementById('animate-button').addEventListener('click', function () {
		writer.animateCharacter();
	});
} 



