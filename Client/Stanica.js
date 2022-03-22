import {Firma} from "./Firma.js";
import {Destinacija} from "./Destinacija.js";
import {PutnikZaPrikaz} from "./PutnikZaPrikaz.js";
import{Ruta} from "./Ruta.js";
import{Putnik} from "./Putnik.js";

export class Stanica
{
    constructor(id,imeStanice,imeGrada,listaTipovaSedista)
    {
        this.id=id;
        this.imeStanice=imeStanice;
        this.imeGrada=imeGrada;

        this.listaRuta=[];
        this.listaDestinacija=[];
        this.listaFirmi=[];
        this.listaPutnika=[];
        this.kont=null;

        // za select
        this.listaTipovaSedista=listaTipovaSedista;
      
        this.listaRazlicitihDestinacija=[];
        this.listaRazlicitihDatuma=[];
    }

    dodajRutu(r)
    {
        this.listaRuta.push(r);
    }
    dodajDestinaciju(d)
    {
        this.listaDestinacija.push(d);
    }
    dodajFirmu(f)
    {
        this.listaFirmi.push(f);
    }
    dodajPutnikaa(p)
    {
        this.listaPutnika.push(p);
    }
    
    crtaj(host) 
    {
        var r =  document.createElement("h3");
        r.innerHTML=this.imeStanice;
        r.className="Naslov";
        host.appendChild(r);

        this.kont=document.createElement("div");
        this.kont.className="GlavniKontejner";
        this.kont.classList.add("kont");
        host.appendChild(this.kont);

        let kontForma = document.createElement("div");
        kontForma.className="Forma";
        this.kont.appendChild(kontForma);
        
        this.crtajFormu(kontForma);
        this.crtajPrikaz(kontForma);
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        let kontDodaj = document.createElement("div");
        kontDodaj.className="Dodavanje";
        this.kont.appendChild(kontDodaj);

        
        let kontIzbrisi = document.createElement("div");
        kontIzbrisi.className="Brisanje";
        this.kont.appendChild(kontIzbrisi);

        
        let kontIzmena = document.createElement("div");
        kontIzmena.className="Izmena";
        this.kont.appendChild(kontIzmena);

        this.CrtajZaDodavanjePutnika(kontDodaj);
        
        this.CrtajZaObrisiPutnika(kontIzbrisi);
        this.CrtajZaIzmeniTipSedista(kontIzmena);
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
    }

    crtajPrikaz(host){

        let kontPrikaz = document.createElement("div");
        kontPrikaz.className="Prikaz";
        host.appendChild(kontPrikaz);

        var tabela = document.createElement("table");
        tabela.className="tabela";
        kontPrikaz.appendChild(tabela);

        var tabelahead= document.createElement("thead");
        tabela.appendChild(tabelahead);

        var tr = document.createElement("tr");
        tabelahead.appendChild(tr);

        var tabelaBody = document.createElement("tbody");
        tabelaBody.className="TabelaPodaci" + this.id;
        tabela.appendChild(tabelaBody);

        let th;
        var zaTab=["Ime", "Prezime","JMBG", "Ime kompanije", "Tip sedista", "Cena"];
        zaTab.forEach(el=>{
            th = document.createElement("th");
            th.innerHTML=el;
            tr.appendChild(th);
        })
    }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    crtajRed(host){
        let red = document.createElement("div");
        red.className="red";
        host.appendChild(red);
        return red;
    }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
     
    
    crtajFormu(host){
        
    
        
        //odaberi grad labela
        let red = this.crtajRed(host);
        let l = document.createElement("label");
        l.innerHTML="Odaberi grad: ";
        l.className="Odaberi";
        red.appendChild(l);

        //select za grad
        let se = document.createElement("select");
        se.className="SelectZaGrad";
        red.appendChild(se);

        se.onchange=(ev)=>{
            let selT=this.kont.querySelector(".SelectZaDatum");
            let options=selT.querySelectorAll("option");
            if(options.length>0){
                options.forEach(option=>{
                    selT.removeChild(option);
                })
            let xd=this.kont.querySelector(".SelectZaGrad");
            let vrednost=xd.options[xd.selectedIndex].innerHTML;
    
            fetch("https://localhost:5001/Destinacija/PreuzmiVremena?grad="+vrednost,
            { 
                method: "GET"
            })
            .then(gg => {
                    gg.json().then(bg=>{
                        console.log(bg.datum+" proba")
                        let op=document.createElement("option");
                        op.innerHTML=bg.datum;
                        op.value=bg.datum;
                        selT.appendChild(op);
                  
                    })
                })
            
            }
        }
        
        this.listaDestinacija.forEach(d=>{
             if(!this.listaRazlicitihDestinacija.includes(d.grad)) 
                  this.listaRazlicitihDestinacija.push(d.grad)
          })

         console.log("Destinacije: "+this.listaRazlicitihDestinacija)

        let op;
        this.listaDestinacija.forEach(d=>{
            op = document.createElement("option");
            op.innerHTML=d.grad;
            op.value=d.id;
            se.appendChild(op);
        })

         //odaberi datum labela
        red = this.crtajRed(host);
        l = document.createElement("label");
        l.innerHTML="Odaberi datum:";
        l.className="Odaberi";
        red.appendChild(l);

        //select za datum
        let dat = document.createElement("select");
        dat.className="SelectZaDatum";
        red.appendChild(dat);

        
    

       
        // this.listaRazlicitihDatuma.forEach(d=>{
        //    if(!this.listaRazlicitihDatuma.includes(d.datumiVreme)) 
        //        this.listaRazlicitihDatuma.push(d.datumiVreme)
        // })
        
        let d=this.listaDestinacija[0];
    op = document.createElement("option");
    op.innerHTML=d.datumiVreme;
    op.value=d;
    dat.appendChild(op); 
        
         //dugme nadji putnike
        red = this.crtajRed(host);
        let btnNadji = document.createElement("button");
        btnNadji.onclick=(ev)=>this.nadjiPutnike();
        btnNadji.innerHTML="Nadji putnike";
        btnNadji.className="Dugme";
        red.appendChild(btnNadji);
        
    }
    
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

CrtajZaDodavanjePutnika(host)
{
    // Ime
    var red = this.crtajRed(host);
    var l =  document.createElement("label");
    l.innerHTML="Ime:"
    red.appendChild(l);
    var Ime = document.createElement("input");
    Ime.type="string";
    Ime.className="KlasaIme";
    red.appendChild(Ime);

    // Prezime
    red = this.crtajRed(host);
    l =  document.createElement("label");
    l.innerHTML="Prezime:"
    red.appendChild(l);
    var Prezime = document.createElement("input");
    Prezime.type="string";
    Prezime.className="KlasaPrezime";
    red.appendChild(Prezime);

    // JMBG
    red = this.crtajRed(host);
    l =  document.createElement("label");
    l.innerHTML="JMBG:"
    red.appendChild(l);
    var Jmbg = document.createElement("input");
    Jmbg.type="string";
    Jmbg.className="KlasaJMBG";
    red.appendChild(Jmbg);

    // Broj telefona
    red = this.crtajRed(host);
    l =  document.createElement("label");
    l.innerHTML="Broj telefona:"
    red.appendChild(l);
    var BrojTelefona = document.createElement("input");
    BrojTelefona.type="string";
    BrojTelefona.className="KlasaBrojTelefona";
    red.appendChild(BrojTelefona);

    // Odaberi grad
    red = this.crtajRed(host);
    l = document.createElement("label");
    l.innerHTML="Odaberi grad:";
    l.className="Odaberi";
    red.appendChild(l);

    
    let se = document.createElement("select");
    se.className="SelectZaGrad1";
    se.onchange=(ev)=>{
        let selT=this.kont.querySelector(".SelectZaDatum1");
        let options=selT.querySelectorAll("option");
        if(options.length>0){
            options.forEach(option=>{
                selT.removeChild(option);
            })
        let xd=this.kont.querySelector(".SelectZaGrad1");
        let vrednost=xd.options[xd.selectedIndex].value;

        fetch("https://localhost:5001/Destinacija/PreuzmiVremena?grad="+vrednost,
        { 
            method: "GET"
        })
        .then(gg => {
                gg.json().then(bg=>{
                    console.log(bg.datum+" proba")
                    let op=document.createElement("option");
                    op.innerHTML=bg.datum;
                    op.value=bg.datum;
                    selT.appendChild(op);
              
                })
            })
        
        }
    }
    red.appendChild(se);

    let op;
    this.listaRazlicitihDestinacija.forEach(d=>{
    op = document.createElement("option");
    op.innerHTML=d;
    op.value=d;
    se.appendChild(op);})    

    // Odaberi datum
    red = this.crtajRed(host);
    l = document.createElement("label");
    l.innerHTML="Odaberi vreme:";
    l.className="Odaberi";
    red.appendChild(l);

    se = document.createElement("select");
    se.className="SelectZaDatum1";
    red.appendChild(se);
   
//    this.listaDestinacija.forEach(d=>{
    let d=this.listaDestinacija[0];
    op = document.createElement("option");
    op.innerHTML=d.datumiVreme;
    op.value=d;
    se.appendChild(op); 
// })

    // Odaberi tip sedista
    red = this.crtajRed(host);
    l = document.createElement("label");
    l.innerHTML="Odaberi tip sedista:";
    l.className="Odaberi";
    red.appendChild(l);

    let fx = document.createElement("select");
    fx.className="SelectZaTipSedista";
    red.appendChild(fx);


    this.listaTipovaSedista.forEach(ts=>{
    op = document.createElement("option");
    op.innerHTML=ts;
    op.value=ts;
    fx.appendChild(op);})
 
    // Dugme za dodavanje putnika
    red = this.crtajRed(host);
    let btnDodajPutnika = document.createElement("button");
    btnDodajPutnika.onclick=(ev)=>this.dodajPutnika(Ime.value,Prezime.value,Jmbg.value,BrojTelefona.value);
    btnDodajPutnika.innerHTML="Zakazi kartu";
    btnDodajPutnika.className="Dugme";
    red.appendChild(btnDodajPutnika);
}

dodajPutnika(ime,prezime,jmbg,BrojTelefona)
{
    // Provera da li su ispravno uneti podaci
    if(ime===null || ime ==="" )
    {
        alert("Unesite ime");
        return;
    }
    if(prezime===null || prezime ==="" )
    {
        alert("Unesite prezime");
        return;
    }
    if(jmbg===null || jmbg==="" )
    {
        alert("Unesite JMBG");
        return;
    }
    else
    {
        if(jmbg.length!=13)
        {
            alert("Neispravna vrednost je uneta za JMBG");
            return;
        }
    }
    if(BrojTelefona===null || BrojTelefona ==="" )
    {
        alert("Unesite broj telefona ");
        return;
    }

    // Preuzimanje vrednosti za grad
    let optionEl = this.kont.querySelector(".SelectZaGrad1");
    var grad = optionEl.options[optionEl.selectedIndex].value; // ime grada

    // Preuzimanje vrednosti za datum
    optionEl = this.kont.querySelector(".SelectZaDatum1");
    var datum = optionEl.options[optionEl.selectedIndex].innerHTML; //datum


    // Preuzimanje vrednosti za tip sedista
    optionEl = this.kont.querySelector(".SelectZaTipSedista");
    var tipSedista = optionEl.options[optionEl.selectedIndex].value;

    //kreiranje putnika koji se prosledjuje preko body-ija
   
    var putnik=new Putnik(ime, prezime, jmbg, BrojTelefona, this.id );
  
    // ---------------------------- FETCH FJE -----------------------------

    // dodavanje rute
    var cenaSedista;
    if(tipSedista == "AC1")
        cenaSedista= 2500;
    else if(tipSedista == "AC2")
        cenaSedista= 1200;
    else if(tipSedista == "AC3")
        cenaSedista=650;

    var rutt=new Ruta(tipSedista,cenaSedista,this.id);
    console.log(rutt.tipSedista,rutt.cenaSedista);
    console.log(putnik);
    fetch("https://localhost:5001/Putnik/DodatiPutnikaFromBody?grad="+ grad +"&ime="+putnik.ime
    +"&prezime="+putnik.prezime+"&jmbg="+putnik.jmbg+"&brTel="+putnik.brTelefona+"&id="+putnik.stanicaId+"&tS="+tipSedista+"&cena="+cenaSedista).then(p=>{
        if(p.ok)
        {
            console.log("USLO U POST - dodati putnika")
           //this.listaPutnika.push(putnik);
           this.dodajPutnikaa(putnik);
        }
        else console.log(" NE ULAZI U POST - dodati putnika")

 


  });  
  }

//
CrtajZaObrisiPutnika(host)
{
      //odaberi grad labela
      let red = this.crtajRed(host);
      let l = document.createElement("label");
      l.innerHTML="Odaberi grad: ";
      l.className="Odaberi";
      red.appendChild(l);

      //select za grad
      let se = document.createElement("select");
      se.className="SelectZaGrad2";

      se.onchange=(ev)=>{
        let selT=this.kont.querySelector(".SelectZaDatum2");
        let options=selT.querySelectorAll("option");
        if(options.length>0){
            options.forEach(option=>{
                selT.removeChild(option);
            })
        let xd=this.kont.querySelector(".SelectZaGrad2");
        let vrednost=xd.options[xd.selectedIndex].innerHTML;

        fetch("https://localhost:5001/Destinacija/PreuzmiVremena?grad="+vrednost,
        { 
            method: "GET"
        })
        .then(gg => {
                gg.json().then(bg=>{
                    console.log(bg.datum+" proba")
                    let op=document.createElement("option");
                    op.innerHTML=bg.datum;
                    op.value=bg.datum;
                    selT.appendChild(op);
              
                })
            })
        
        }
    }
      red.appendChild(se);

      
      

      let op;
      this.listaRazlicitihDestinacija.forEach(d=>{
          op = document.createElement("option");
          op.innerHTML=d;
          op.value=d;
          se.appendChild(op);
      })

       //odaberi datum labela
      red = this.crtajRed(host);
      l = document.createElement("label");
      l.innerHTML="Odaberi datum:";
      l.className="Odaberi";
      red.appendChild(l);

      //select za datum
      let sf = document.createElement("select");
      sf.className="SelectZaDatum2";
      red.appendChild(sf);

    // this.listaDestinacija.forEach(d=>{
        let d=this.listaDestinacija[0];
        op = document.createElement("option");
        op.innerHTML=d.datumiVreme;
        op.value=d;
        sf.appendChild(op); 
    // })
      

    //Unesite JMBG
    red = this.crtajRed(host);
    l =  document.createElement("label");
    l.innerHTML="Unesite JMBG:";
    red.appendChild(l);
    var JMBG = document.createElement("input");
    JMBG.type="string";
    JMBG.className="KlasaJMBG";
    red.appendChild(JMBG);

    //Dugme OBRISI
      red = this.crtajRed(host);
      let btnObrisiPutnika = document.createElement("button");
      btnObrisiPutnika.onclick=(ev)=>this.obrisiPutnika(JMBG.value);
      btnObrisiPutnika.innerHTML="Obrisi putnika";
      btnObrisiPutnika.className="Dugme";
      red.appendChild(btnObrisiPutnika);

}

obrisiPutnika(jmbg)
{
    if(jmbg===null || jmbg==="" )
    {
        alert("Unesite JMBG!");
        return;
    }
    else
    {
        if(jmbg.length!=13)
        {
            alert("Neispravna vrednost je uneta za JMBG!");
            return;
        }
    } 
    // Preuzimanje vrednosti za grad
    let optionEl = this.kont.querySelector(".SelectZaGrad2");
    var grad = optionEl.options[optionEl.selectedIndex].value; // ime grada

    // Preuzimanje vrednosti za datum
    optionEl = this.kont.querySelector(".SelectZaDatum2");
    var datum = optionEl.options[optionEl.selectedIndex].value; //datum

  fetch("https://localhost:5001/Putnik/IzbrisiPutnika/"+jmbg +"/"+ grad +"/"+datum,

  {
      method: "DELETE"
  }) .then(p=>
    {
        if(p.ok)
        {
            var teloTabele = this.obrisiPrethodniSadrzaj();
            this.nadjiPutnike();

            console.log("Obrisan putnik");
        }
        else alert("Ne postoji putnik sa unetim JMBG-om!");
 
        

    });

}

// ---------------------------------- IZMENI TIP SEDISTA -------------------------------------

CrtajZaIzmeniTipSedista(host)
{
    //Unesite broj licne karte
    var red = this.crtajRed(host);
    var l =  document.createElement("label");
    l.innerHTML="Unesite broj JMBG-a:";
    red.appendChild(l);
    var brojJMBG = document.createElement("input");
    brojJMBG.type="string";
    brojJMBG.className="KlasaBrojJMBG";
    red.appendChild(brojJMBG);
    //Odaberite novi tip sedista
    red = this.crtajRed(host);
    l = document.createElement("label");
    l.innerHTML="Odaberi novi tip sedista:";
    l.className="Odaberi";
    red.appendChild(l);

    var se = document.createElement("select");
    se.className="SelectZaTipSedista1";
    red.appendChild(se);

    this.listaTipovaSedista.forEach(ts=>{
    var op = document.createElement("option");
    op.innerHTML=ts;
    op.value=ts;
    se.appendChild(op);
    })

    
    red = this.crtajRed(host);
    let btnIzmeniTipSedista = document.createElement("button");
    btnIzmeniTipSedista.onclick=(ev)=>this.izmeniTipSedista(brojJMBG.value);
    btnIzmeniTipSedista.innerHTML="Izmeni tip sedista";
    btnIzmeniTipSedista.className="Dugme";
    red.appendChild(btnIzmeniTipSedista);
}

izmeniTipSedista(brojJMBG)  
{

    console.log(brojJMBG + "ve;jko")

    let pomoc=this.kont.querySelector(".SelectZaTipSedista1");
    let tipSedista=pomoc.options[pomoc.selectedIndex].value;

    if(brojJMBG===null || brojJMBG ==="" )
    {
        alert("Unesite broj licne karte");
        return;
    }
    else
    {
        if(brojJMBG.length!=13)
        {
            alert("Neispravna vrednost je uneta za JMBG");
            return;
        }
    }

    fetch("https://localhost:5001/Putnik/IzmeniTipSedista/"+tipSedista+"/"+brojJMBG,
    {
        method:"PUT",
         headers:{
            "Content-Type":"application/json"
        }
    }).then(p=>{
        if(p.ok){
            this.nadjiPutnike();
            
         

            console.log("Uspesno izmenjen tip sedista"+tipSedista);
        }
        else alert("Ne postoji putnik sa ovom licnom kartom!");

    })}


    nadjiPutnike(){

        let optionEl = this.kont.querySelector(".SelectZaGrad");
        var gradID=optionEl.options[optionEl.selectedIndex].value;
        console.log(gradID+"ZUBBBB");

        optionEl = this.kont.querySelector(".SelectZaDatum");
        var datumiVremeID=optionEl.options[optionEl.selectedIndex].value;
        console.log(datumiVremeID+"dosta viseeee");
       
        this.ucitajPutnike(datumiVremeID,gradID);
    }


    ucitajPutnike(datumiVremeID,gradID){

        // string datumiVreme, string grad,int idStanice
        console.log(typeof datumiVremeID);
        fetch("https://localhost:5001/Putnik/PrikaziPutnikeKojiPutuju?datumiVreme="+datumiVremeID+"&grad="+gradID +"&idStanice"+this.id,
        {
            method:"GET"
        }).then(p=>{
            if(p.ok){
                var teloTabele = this.obrisiPrethodniSadrzaj();
                p.json().then(data=>{
                    data.forEach(p=>{
                        console.log(p.ime,p.prezime,p.jmbg,p.brLicneKarte,p.imeFirme, p.tipSedista, p.cena)
                        let put = new PutnikZaPrikaz(p.ime,p.prezime,p.jmbg,p.brLicneKarte,p.imeFirme, p.tipSedista, p.cena);
                        put.crtaj(teloTabele);
                    })
                    
                })
            }
        })
    }
    
        obrisiPrethodniSadrzaj(){
            var teloTabele = document.querySelector(".TabelaPodaci"+ this.id);
            var roditelj = teloTabele.parentNode;
            roditelj.removeChild(teloTabele);
    
            teloTabele = document.createElement("tbody");
            teloTabele.className="TabelaPodaci"+ this.id;
            roditelj.appendChild(teloTabele);
            return teloTabele;
        }
    
        obrisiPrethodniSadrzajZaGraf(){
            var graf = document.querySelector(".Graf");
            var roditelj = graf.parentNode;
            roditelj.removeChild(graf);
        }


}
