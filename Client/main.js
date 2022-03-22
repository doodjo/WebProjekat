import {Stanica} from "./Stanica.js";
import {Firma} from "./Firma.js";
import {Destinacija} from "./Destinacija.js";
import {Putnik} from "./Putnik.js";
import {Ruta} from "./Ruta.js";


var listaTipovaSedista=[];
fetch("https://localhost:5001/Putnik/PreuzmiTipoveSedista")
    .then(p=>{
        p.json().then(tipovi=>{
            tipovi.forEach(tip => {
                if(!listaTipovaSedista.includes(tip))
                 listaTipovaSedista.push(tip);
            })
           
                 var listaRuta=[];
                fetch("https://localhost:5001/Ruta/PreuzmiRuteZaStanicu")
                .then(p=>{
                p.json().then(rute=>{
                rute.forEach(rut => {
                var rut= new Ruta(rut.tipSedista, rut.cena, rut.stanica.id); 
                listaRuta.push(rut);
                })

                                var listaPutnika=[];
                                fetch("https://localhost:5001/Putnik/PreuzmiPutnikeZaStanicu")
                                .then(p=>{
                                p.json().then(putnici=>{
                                putnici.forEach(p => {
                                console.log("IME: " + p.ime)
                                var pu= new Putnik(p.ime,p.prezime,p.jmbg,p.brTelefona,p.stanica); 
                                listaPutnika.push(pu);
                                                    })

                                        var listaDestinacija =[];
                                        fetch("https://localhost:5001/Destinacija/PreuzmiDestinacijeZaStanicu")
                                        .then(p=>{
                                        p.json().then(destinacija=>{
                                        destinacija.forEach(de => {
                                        var p= new Destinacija(de.id, de.drzava, de.grad, de.datum, de.stanica.id); 
                                        listaDestinacija.push(p);
                                        console.log("Veljko"+p.datumiVreme);
                                                                    })


                                                    var listaFirmi =[];
                                                    fetch("https://localhost:5001/Firma/PreuzmiFirmeZaStanicu")
                                                    .then(r=>{
                                                    r.json().then(firm=>{
                                                    firm.forEach(firm => {
                                                    var k = new Firma(firm.id, firm.imeFirme, firm.brSedista, firm.stanica.id);
                                                    listaFirmi.push(k);    
                                                                    })

                                                                    var listaStanica =[];
                                                                    fetch("https://localhost:5001/Stanica/PreuzmiStanice")
                                                                    .then(p=>{
                                                                    p.json().then(station=>{
                                                                    station.forEach(st=> {
                                                                    var a =new Stanica(st.id, st.imeStanice, st.imeGrada, listaTipovaSedista);
                                                        
                                                                    listaStanica.push(a);
                                                                            
                                                                    listaRuta.forEach(l=>{
                                                                            if(l.stanicaId==st.id)
                                                                            a.dodajRutu(l);
                                                                                        })
                                                                            
                                                                            listaPutnika.forEach(l=>{
                                                                            if(l.stanicaId==st.id)
                                                                            a.dodajPutnikaa(l);
                                                                                        })
                                                                            
                                                                            listaFirmi.forEach(l=>{
                                                                            if(l.stanicaId==st.id)
                                                                            a.dodajFirmu(l);
                                                                                        })
                                                                            
                                                                            listaDestinacija.forEach(l=>{
                                                                            if(l.stanicaId==st.id)
                                                                            a.dodajDestinaciju(l);
                                                                                        })
                                                                            a.crtaj(document.body);
                                                                                    })
                                                                                console.log(listaStanica);
                                                                                }) 
                                                                            })
                                                                        })
                                                                    })
                                                                }) 
                                                            })
                                                }) 
                             })
                     }) 
             })
     }) 

})

    
// var listaRuta=[];
// fetch("https://localhost:5001/Ruta/PreuzmiRuteZaStanicu")
// .then(p=>{
//     p.json().then(rute=>{
//         rute.forEach(rut => {
//             var rut= new Ruta(rut.tipSedista, rut.cena, rut.stanica.id); 
//             listaRuta.push(rut);
//         })
//     }) 
// })

// var listaPutnika=[];
// fetch("https://localhost:5001/Putnik/PreuzmiPutnikeZaStanicu")
// .then(p=>{
//     p.json().then(putnici=>{
//         putnici.forEach(p => {
//             console.log("IME: " + p.ime)
//             var pu= new Putnik(p.ime,p.prezime,p.jmbg,p.brTelefona,p.stanica); 
//             listaPutnika.push(pu);
//         })
//         console.log(listaPutnika);
//     }) 
// })


// var listaDestinacija =[];
// fetch("https://localhost:5001/Destinacija/PreuzmiDestinacijeZaStanicu")
// .then(p=>{
//     p.json().then(destinacija=>{
//         destinacija.forEach(de => {
//             var p= new Destinacija(de.id, de.drzava, de.grad, de.datum, de.stanica.id); 
//                   listaDestinacija.push(p);
//                   console.log("Veljko"+p.datumiVreme);
//         })
//     }) 
// })

// var listaFirmi =[];
// fetch("https://localhost:5001/Firma/PreuzmiFirmeZaStanicu")
// .then(r=>{
//     r.json().then(firm=>{
//         firm.forEach(firm => {
//             var k = new Firma(firm.id, firm.imeFirme, firm.brSedista, firm.stanica.id);
//             listaFirmi.push(k);    
//         })
//     })
// })

// var listaStanica =[];
// fetch("https://localhost:5001/Stanica/PreuzmiStanice")
// .then(p=>{
//     p.json().then(station=>{
//         station.forEach(st=> {
//             var a =new Stanica(st.id, st.imeStanice, st.imeGrada, listaTipovaSedista);
//             //console.log(a);
//             listaStanica.push(a);

//             listaRuta.forEach(l=>{
//                 if(l.stanicaId==st.id)
//                 a.dodajRutu(l);
//             })

//             listaPutnika.forEach(l=>{
//                 if(l.stanicaId==st.id)
//                 a.dodajPutnikaa(l);
//             })

//             listaFirmi.forEach(l=>{
//                 if(l.stanicaId==st.id)
//                 a.dodajFirmu(l);
//             })

//             listaDestinacija.forEach(l=>{
//                 if(l.stanicaId==st.id)
//                    a.dodajDestinaciju(l);
//             })
//             a.crtaj(document.body);
//         })
//         console.log(listaStanica);
//     }) 
// })


// console.log(listaStanica)