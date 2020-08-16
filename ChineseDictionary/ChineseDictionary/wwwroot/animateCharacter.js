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

function writting(character, color) {
	numbOfMistakes = 0;
	document.getElementById('mistakesCounter').innerHTML = 'You have made 0 mistakes';
	document.getElementById('mistakesCounter').style.color = '#000000';
	var writer = HanziWriter.create('character-quizzing', character, {
		width: 100,
		height: 100,
		showCharacter: false,
		padding: 5,
		strokeColor: color
	});
	writer.quiz({
		onMistake: function () {
			numbOfMistakes++;
			DotNet.invokeMethodAsync('ChineseDictionary', 'Mistakes');
			var div = document.getElementById('mistakesCounter');
			div.innerHTML = 'You have made ' + numbOfMistakes + ' mistakes';
			if (numbOfMistakes > 3)
				document.getElementById('mistakesCounter').style.color = '#FF0000';
		},
		onComplete: function () {
			DotNet.invokeMethodAsync('ChineseDictionary', 'Complete')
		}
	});
}

function removeListenersQuiz() {
	document.getElementById('character-quizzing').innerHTML = "";
}