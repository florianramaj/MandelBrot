
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-mainpage',
  templateUrl: './mainpage.component.html',
  styleUrls: ['./mainpage.component.scss']
})
export class MainpageComponent implements OnInit {
  valueSlide2=4;
  valueSlide1=2;
  constructor() {

  }

  ngOnInit() {

    this.draw();

  }

  firstSliderChanged()
  {
    this.draw();
  }

  secondSliderChanged()
  {
    this.draw();
  }

  draw() {

    let canvas = document.getElementById("canvas") as HTMLCanvasElement;
    let context = canvas.getContext("2d");
    let maxIt = 100;
    let maxValue = 4; //eigentlich 2 aber so muss ich ned wurzel ziehen lel
    let verschiebungX = canvas.width / 2;
    let verschiebungY = canvas.height / 2;

    for (let index = 0; index < canvas.width; index++) {
      for (let index2 = 0; index2 < canvas.height; index2++) {
        let counter = 0;

        // f(z) = z^2 + c
        let c = 0;
        let betrag = 0;
        let realValue =0;
        let imagValue=0;
        let cReal = (index -(canvas.width / this.valueSlide1)) / (canvas.width/this.valueSlide2);
        let cImag = (index2 -(canvas.height / this.valueSlide1)) / (canvas.height/this.valueSlide2);
       


        while (counter < maxIt) {
          let realValueHelp = realValue * realValue - imagValue * imagValue;
          let imagValueHelp = 2 * realValue * imagValue;
          



          realValue = realValueHelp + cReal;
          imagValue = imagValueHelp + cImag;

          betrag = realValue * realValue + imagValue * imagValue;

          if(betrag > maxValue)
          {
            break;
          }

          
          
          counter++;
        }

        context.fillStyle = "blue"

        if(counter == maxIt)       
        {
          context.fillStyle = "red"
          
        }
        context.fillRect(index, index2, 1, 1);


       // context.fillStyle = "red"
        //context.fillRect(verschiebungX + index, verschiebungY - index2, 1, 1);
      }

    }









  }

  myMap(n, start1, stop1, start2, stop2) {
    return ((n - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
  }


}
