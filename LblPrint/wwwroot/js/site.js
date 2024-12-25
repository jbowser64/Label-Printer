// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


const partNumber = document.getElementById('partNum');
const labelNum = document.getElementById('lblPartNum');
function myfun() {
    labelNum = partNumber.ariaValueText;
}
partNumber.addEventListener(onchange, myfun());