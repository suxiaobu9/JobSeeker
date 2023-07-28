window.registerShortcut = (dotNetReference) => {
    document.addEventListener('keydown', (event) => {
        if (event.key === 'q') {
            dotNetReference.invokeMethodAsync('ReadAllJobs');
        }
        if (event.key === 'w') {
            dotNetReference.invokeMethodAsync('IgnoreCompany');
        }
        if (event.key === 'a') {
            dotNetReference.invokeMethodAsync('ReadFirstJob');
        }
        if (event.key === 's') {
            dotNetReference.invokeMethodAsync('IgnoreFirstJob');
        }
    });
}
