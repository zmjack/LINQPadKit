let $console = {
    element: document.getElementById('console'),
    log: function (s: string) {
        let p = document.createElement('p');
        p.innerText = s;
        (this.element as HTMLElement).appendChild(p);
    },
    text: function (s: string) {
        let p = document.createElement('p');
        p.innerText = s;
        (this.element as HTMLElement).innerHTML = null;
        (this.element as HTMLElement).appendChild(p);
    }
}

var a = 
{
    f: function() {
        $console.log(this.name);
    },
    name: 'jack',
}

var b = 
{
    name: 'lily',
}

a.f();
var ff = a.f.bind(b);

ff();
