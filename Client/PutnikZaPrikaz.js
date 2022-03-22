export class PutnikZaPrikaz
{
    constructor(ime, prezime, jmbg, imeFirme, tipSedista, cena)
    {   
        this.ime=ime;
        this.prezime=prezime;
        this.jmbg=jmbg;
        this.imeFirme=imeFirme;
        this.tipSedista=tipSedista;
        this.cena=cena;
    }

     crtaj(host)
     {
        var tr=document.createElement("tr");
        tr.className="MainRow"
        host.appendChild(tr);
        
        var el=document.createElement("td");
        el.innerHTML=this.ime;
        tr.appendChild(el);

        el=document.createElement("td");
        el.innerHTML=this.prezime;
        tr.appendChild(el);

        el=document.createElement("td");
        el.innerHTML=this.jmbg;
        tr.appendChild(el);

        el=document.createElement("td");
        el.innerHTML=this.imeKompanije;
        tr.appendChild(el);

        el=document.createElement("td");
        el.innerHTML=this.tipSedista;
        tr.appendChild(el);

        el=document.createElement("td");
        el.innerHTML=this.cena;
        tr.appendChild(el);
     }
}