window.registerShortcut = (dotNetReference) => {
    document.addEventListener('keydown', (event) => {
        if (event.key === 'q') {
            dotNetReference.invokeMethodAsync('HandleKeyPress');
        }
        if (event.key === 'w') {
            document.querySelector('#btn-jobs').click();
        }
        if (event.key === 'a') {
            document.querySelector('#read-job').click();
        }
        if (event.key === 's') {
            document.querySelector('#ignore-job').click();
        }
        if (event.key === 'z') {
            document.querySelector('#read-comp').click();
        }
        if (event.key === 'x') {
            document.querySelector('#ignore-comp').click();
        }
    });
}