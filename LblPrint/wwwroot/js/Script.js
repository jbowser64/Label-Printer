// Print Labels functionality
document.addEventListener('DOMContentLoaded', function () {
    const printButton = document.getElementById('btnPrintLabels');
    const printLabelForm = document.getElementById('printLabelForm');
    const printNumInput = document.getElementById('printNum');
    const qtyInput = document.getElementById('QTYLbl');
    const fifoInput = document.getElementById('FIFO');
    const binInput = document.getElementById('Loc');

    if (printButton && printLabelForm) {
        // Handle print button click
        printButton.addEventListener('click', function () {
            // Update form values from input fields
            const printNumHidden = document.getElementById('printNumHidden');
            const qtyHidden = document.getElementById('qtyHidden');
            const fifoHidden = document.getElementById('fifoHidden');
            const binHidden = document.getElementById('binHidden');

            if (printNumHidden) {
                printNumHidden.value = printNumInput.value.trim();
            }

            if (qtyHidden) {
                qtyHidden.value = parseInt(qtyInput.value) || 1;
            }

            if (fifoHidden) {
                const fifoValue = fifoInput.value.trim();
                fifoHidden.value = fifoValue || '';
            }

            if (binHidden) {
                const binValue = binInput.value.trim();
                binHidden.value = binValue || '';
            }

            // Disable button and show loading state
            printButton.disabled = true;
            const originalText = printButton.textContent;
            printButton.textContent = 'Generating Label...';

            // Submit the form
            printLabelForm.submit();

            // Re-enable button after a delay (in case of error)
            setTimeout(() => {
                printButton.disabled = false;
                printButton.textContent = originalText;
            }, 3000);
        });
    }
});

