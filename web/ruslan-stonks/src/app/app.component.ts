import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent implements OnInit {
  title = 'ruslan-stonks';
  lineChart = [];

  ngOnInit(): void {
    this.lineChart = new Chart(
      'lineChart', {
        type : 'line', 
        data : {
          labels : ['January', 'February', 'March', 'April', 'May'],
          datasets : [
            {
              label : 'Number of items sold in month',
              data : [0, 1, 2, 5, 4],
              fill : true, 
              lineTension : 0.2, 
              borderColor : 'red',
              borderWidth : 1, 
            }
          ]
        },
        options : {
          title : {
            text : 'Angular LineChart',
            display : true
          },
          scales : {
            yAxes : [{
              ticks : {
                beginAtZero : true
              }
            }]
          }
        }
      }
    );
  }
}
