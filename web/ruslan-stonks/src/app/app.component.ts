import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.sass']
})
export class AppComponent implements OnInit {
  title = 'ruslan-stonks';
  lineChart = [];

  constructor(private _httpClient: HttpClient)
  {

  }

  private getMarketData(id: number, from: number, to: number)
  {
    this._httpClient.get(`https://api.ruslan.local/market/generate?companyId=${id}&from=${from}&to=${to}`)
      .subscribe((result: any[]) => console.log(result));
  }

  ngOnInit(): void {
    var data = [];
    this.getMarketData(1, 1, 5);
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
