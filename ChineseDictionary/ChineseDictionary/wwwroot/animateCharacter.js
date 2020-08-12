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

function quizz(character) {
	var writer = HanziWriter.create('character-target-div-quizz', character, {
		width: 100,
		height: 100,
		padding: 5,
		showCharacter: false
	});
	writer.quiz();
	document.getElementById('reset-button').addEventListener('click', function () {
		writer.quiz({
			onMistake: function (strokeData) {
				var elem = document.getElementsByClassName('country');
				var textElem = 'Oh no! you made a mistake on stroke ' + strokeData.strokeNum + 'in character' + strokeData.character + '<br/>' /*+ "You've made " + strokeData.mistakesOnStroke + " mistakes on this stroke so far" + '<br/>' + "You've made " + strokeData.totalMistakes + " total mistakes on this quiz" + '<br/>' + "There are " + strokeData.strokesRemaining + " strokes remaining in this character" +'<br/>'*/;
				listElement = document.createElement('p');
				listElement.innerHTML = textElem;
				elem[0].appendChild(listElement);
			},
			onCorrectStroke: function (strokeData) {
				var elem = document.getElementsByClassName('country');
				var textElem = 'Yes!!! You got stroke ' + strokeData.strokeNum + ' in character ' + strokeData.character+ ' correct!' + '<br/>';
				listElement = document.createElement('p');
				listElement.innerHTML = textElem;
				elem[0].appendChild(listElement);

			},
			onComplete: function (summaryData) {
				var elem = document.getElementsByClassName('country');
				var textElem = 'You did it! You finished drawing ' + summaryData.character + '<br/>' + 'You made ' + summaryData.totalMistakes + ' total mistakes on this quiz' +'<br/>';
				listElement = document.createElement('p');
				listElement.innerHTML = textElem;
				elem[0].appendChild(listElement);
			}
		});
	});

}