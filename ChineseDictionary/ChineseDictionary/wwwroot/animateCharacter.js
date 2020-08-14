function drawCharacter(character, color) {
	var writer = HanziWriter.create('character-target-div', character, {
		width: 100,
		height: 100,
		padding: 5,
		showCharacter: true,
		strokeColor: color
	});
	document.getElementById('animate-button').addEventListener('click', function () {
		writer.animateCharacter();
	});
}

function removeListeners() {
	document.getElementById('character-target-div').innerHTML = "";
}


function writtingCharacter(character, color) {
	var writer = HanziWriter.create('character-quizzing', character, {
		width: 100,
		height: 100,
		showCharacter: false,
		padding: 5,
		strokeColor: color
	});
	writer.quiz({
		onMistake: function () {
			DotNet.invokeMethodAsync('ChineseDictionary', 'Mistakes')
		},
		onComplete: function () {
			DotNet.invokeMethodAsync('ChineseDictionary', 'Complete')
		}
	});
}

function removeListenersQuiz() {
	document.getElementById('character-quizzing').innerHTML = "";
}