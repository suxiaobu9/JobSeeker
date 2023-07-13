window.registerShortcut = (dotNetReference) => {
    document.addEventListener('keydown', (event) => {
        if (event.key === 'q') {
            dotNetReference.invokeMethodAsync('ReloadJobs');
        }
        if (event.key === 'w') {
            document.querySelector('#btn-jobs').click();
        }
        if (event.key === 'a') {
            dotNetReference.invokeMethodAsync('ReadFirstJob');
        }
        if (event.key === 's') {
            dotNetReference.invokeMethodAsync('IgnoreFirstJob');
        }
        if (event.key === 'z') {
            dotNetReference.invokeMethodAsync('ReadCompany');
        }
        if (event.key === 'x') {
            dotNetReference.invokeMethodAsync('IgnoreCompany');
        }
    });
}
