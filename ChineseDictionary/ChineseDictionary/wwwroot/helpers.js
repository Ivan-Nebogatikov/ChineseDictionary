window.getWindowSize = () => {
    return { height: window.innerHeight, width: window.innerWidth };
};

((window) => {
    window.__blazorCanvasInterop = {
        drawLine: (canvas, sX, sY, eX, eY) => {
            let context = canvas.getContext('2d');
            context.lineJoin = 'round';
            context.lineWidth = 5;
            context.beginPath();
            context.moveTo(eX - canvas.offsetLeft, eY - canvas.offsetTop);
            context.lineTo(sX - canvas.offsetLeft, sY - canvas.offsetTop);
            context.closePath();
            context.stroke();
        },
        clearCanvas: (canvas) => {
            let context = canvas.getContext('2d');
            context.clearRect(0, 0, context.canvas.width, context.canvas.height);
        },
        setContextPropertyValue: (canvas, propertyName, propertyValue) => {
            let context = canvas.getContext('2d');
            context[propertyName] = propertyValue;
        }
    };

})(window);
