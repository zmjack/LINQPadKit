var $console = {
    element: document.getElementById('console'),
    log: function (s) {
        var p = document.createElement('p');
        p.innerText = s;
        this.element.appendChild(p);
    },
    text: function (s) {
        var p = document.createElement('p');
        p.innerText = s;
        this.element.innerHTML = null;
        this.element.appendChild(p);
    }
};
var a = {
    f: function () {
        $console.log(this.name);
    },
    name: 'jack',
};
var b = {
    name: 'lily',
};
a.f();
var ff = a.f.bind(b);
ff();
