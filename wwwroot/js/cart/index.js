var cartTable = document.getElementById('cartTable');
var rows = cartTable.children[1].rows;
var totalPrice = document.getElementById('totalPrice');


window.onload = function () {
    function getSubTotal(rows) {
        let tds = rows.cells;
        let unitPrice = parseFloat(tds[1].innerHTML);
        let count = parseInt(rows.getElementsByTagName('input')[0].value);
        let subTotal = parseFloat(unitPrice * count);
        tds[3].innerHTML = subTotal.toFixed(2);
    }

    function getTotal() {
        let sum = 0;
        for (let i = 0; i < rows.length; i++) {
            sum += parseFloat(rows[i].cells[3].innerHTML);
        }
        totalPrice.innerHTML = sum.toFixed(2);
    }

    for (var i = 0; i < rows.length; i++) {
        rows[i].onclick = function (e) {
            e = e || window.event;
            var el = e.srcElement;
            var cls = el.className;
            var input = this.getElementsByTagName('input')[0];
            var val = parseInt(input.value);
            switch (cls) {
                case 'add':
                    input.value = val + 1;
                    getSubTotal(this);
                    break;
                case 'reduce':
                    if (val > 1) {
                        input.value = val - 1;
                    }
                    getSubTotal(this);
                    break;
                case 'removeBtn':
                    var conf = confirm('Do you really want to remove this product?');
                    if (conf) {
                        this.parentNode.removeChild(this);
                    }
                    break;
                default:
                    break;
            }
            getTotal();
        }

        rows[i].getElementsByTagName('input')[0].onkeyup = function () {
            let val = parseInt(this.value);
            let t = this.parentNode.parentNode;
            if (isNaN(val) || val < 1) {
                val = 1;
            }
            this.value = val;
            getSubTotal(t);
            getTotal();
        }
    }
}