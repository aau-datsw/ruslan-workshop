import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:charts_flutter/flutter.dart' as charts;
import 'package:http/http.dart' as http;



void main() => runApp(MyApp());

class MyApp extends StatelessWidget {

  final groupTokens = [
    "bramsdockercomposecirclejerk",
    "66cdfff29584225ac6d1fc8db6f6c01d",
    "66cdfff29584225ac6d1fc8db6f6c01d",
    "66cdfff29584225ac6d1fc8db6f6c01d",
    "66cdfff29584225ac6d1fc8db6f6c01d",
    "66cdfff29584225ac6d1fc8db6f6c01d",
    "66cdfff29584225ac6d1fc8db6f6c01d",
    "66cdfff29584225ac6d1fc8db6f6c01d",
    "66cdfff29584225ac6d1fc8db6f6c01d",
    "66cdfff29584225ac6d1fc8db6f6c01d",
    "66cdfff29584225ac6d1fc8db6f6c01d"
  ];

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      theme: ThemeData(
        // This is the theme of your application.
        //
        // Try running your application with "flutter run". You'll see the
        // application has a blue toolbar. Then, without quitting the app, try
        // changing the primarySwatch below to Colors.green and then invoke
        // "hot reload" (press "r" in the console where you ran "flutter run",
        // or simply save your changes to "hot reload" in a Flutter IDE).
        // Notice that the counter didn't reset back to zero; the application
        // is not restarted.
        primarySwatch: Colors.orange,
      ),
      home: Scaffold(
        body: Column(
          children: <Widget>[
            Expanded(child: MarketOverview(interval: Duration(minutes: 5), rate: Duration(seconds: 1))),
            Expanded(
              child: GridView.count(
                crossAxisCount: 3,
                children: groupTokens.map((token) => GroupCard(xToken: token)).toList(),
              ),
            )
          ],
        )
      )
    );
  }
}


class GroupCard extends StatefulWidget {

  final String xToken;

  const GroupCard({Key key, this.xToken}) : super(key: key);

  @override
  _GroupCardState createState() => _GroupCardState();
}

class _GroupCardState extends State<GroupCard> {

  String name;
  int balance = 0;
  int stockValue = 0;
  int totalValue = 0;

  int lastBalance = 0;

  dynamic info;

  @override
  void initState() {
    print("Updating info for: ${widget.xToken}");
    startUpdateLoop(widget.xToken);
    super.initState();
  }

  @override
  void dispose() {
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(8.0),
      child: Stack(
        alignment: AlignmentDirectional.center,
        children: <Widget>[
          Container(
            width: 200.0,
            height: 200.0,
          ),
          Container(
            alignment: FractionalOffset(20.0, 20.0),
            decoration: BoxDecoration(
              border: Border.all(
                color: info["stonk_count"] > 0 ? Colors.blue.withOpacity(0.5) : Colors.grey.withOpacity(0.5),
                width: 50.0,
              ),
              shape: BoxShape.circle
            ),
            child: Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: <Widget>[
                  Text(info == null ? "?" : info["name"], style: TextStyle(fontSize: 36, fontWeight: FontWeight.bold)),
                  buildEarnings(),
                  buildInfo()
                ],
              )
            ),
          ),
        ],
      ),
    );
  }

  Widget buildEarnings() {
    var earnings = info["total_value"] - 100000;
    var percentageEarnings = (earnings / 100000.0) * 100;
    var earningsString = "${earnings >= 0 ? '+' : ''} ${percentageEarnings.toStringAsFixed(2)}";


    return Padding(
      padding: const EdgeInsets.all(8.0),
      child: Text("$earningsString%", style: TextStyle(fontSize: 38, color: earnings < 0 ? Colors.red : Colors.green)),
    );
  }

  Widget buildInfo() {
    return Column(
      children: <Widget>[
        Text("Total Value", style: TextStyle(fontWeight: FontWeight.bold)),
        Text("\$${info['total_value']}"),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          children: <Widget>[
            Column(
              children: <Widget>[
                Text("Stonks", style: TextStyle(fontWeight: FontWeight.bold)),
                Text("\$${info['stonk_value']}")
              ],
            ),
            Column(
              children: <Widget>[
                Text("Balance", style: TextStyle(fontWeight: FontWeight.bold)),
                Text("\$${info['balance']}")
              ],
            )
          ],
        ),
      ],
    );
  }

  void startUpdateLoop(String xToken) async {
    do {
      await Future.delayed(Duration(seconds: 1), () => updateInfo(xToken));
    } while (true);
  }

  void updateInfo(String xToken) async {
    var url = "http://srv.ruslan.dk:3001/api/v1/account";

    try {
      var response = await http.get(url, headers: {
        "X-Token" : xToken
      });
      var i = jsonDecode(response.body);
      setState(() {
        info = i;
      });
    } on Exception catch (e) {
      print(e);
    }
  }
}




class MarketOverview extends StatelessWidget {
  final Duration interval;
  final Duration rate;

  const MarketOverview({Key key, this.interval, this.rate}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.all(8.0),
      child: Card(
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Column(
            children: <Widget>[
              Expanded(child: StonksMarketChart(animate: true, interval: interval, rate: rate))
            ],
          ),
        ),
        elevation: 16,
      ),
    );
  }
}

class StonksMarketChart extends StatefulWidget {
  
  final bool animate;
  final Duration interval;
  final Duration rate;

  const StonksMarketChart({Key key, this.animate, @required this.interval, @required this.rate}) : super(key: key);

  @override
  _StonksMarketChartState createState() => _StonksMarketChartState();
}

class _StonksMarketChartState extends State<StonksMarketChart> {
  int lastPrice = 0, currentPrice = 0;

  charts.Series<StonksRecord, DateTime> records;

  @override 
  void initState() {
    startUpdateLoop();
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: <Widget>[
        // Headline
        headline(),

        // Updating chart
        Expanded(
          child: charts.TimeSeriesChart(
            records == null ? [] : [records],
            animate: widget.animate,
            dateTimeFactory: const charts.LocalDateTimeFactory(),
            ),
        )
      ],
    );
  }

  Widget headline() {
    var difference = currentPrice - lastPrice;
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: <Widget>[
        Text("RUSLAN Stonks - Ligma Inc.        ", style: Theme.of(context).textTheme.headline),
        Text("\$${currentPrice}.00", style: TextStyle(
          fontSize: 48, 
          color: difference > 0 ? Colors.green : Colors.red
        )),
        difference > 0 ? Icon(Icons.arrow_drop_up, color: Colors.green) : Icon(Icons.arrow_drop_down, color: Colors.red)
      ],
    );
  }

  void startUpdateLoop() async {
    do {
      await Future.delayed(widget.rate, () => updateRecords());
    } while (true);
  }

  void updateRecords() async {
    var to = DateTime.now();
    var from = to.subtract(widget.interval);

    var url = "http://srv.ruslan.dk:3001/api/v1/market?from=${from.toIso8601String()}&to=${to.toIso8601String()}";
    var marketData = List<StonksRecord>();
    try {
      var response = await http.get(url, headers: {"X-Token" : "bramsdockercomposecirclejerk"});
      jsonDecode(response.body).forEach((o) => marketData.add(StonksRecord(
        time:  DateTime.parse(o["recorded"]),
        price: o["price"]
      )));
      
      setState(() {
        if (records != null && records.data.length > 0)
          lastPrice = records.data.last.price;

        records = charts.Series<StonksRecord, DateTime>(
          id: 'Records',
          colorFn: (_, __) => charts.MaterialPalette.blue.shadeDefault,
          domainFn: (StonksRecord records, _) => records.time,
          measureFn: (StonksRecord records, _) => records.price,
          measureLowerBoundFn: (StonksRecord records, _) => records.price - 200,
          measureUpperBoundFn: (StonksRecord records, _) => records.price - 200,
          areaColorFn: (StonksRecord records, _) => charts.ColorUtil.fromDartColor(Colors.blue.withOpacity(0.5)),
          data: marketData
        );

        if (records != null && records.data.length > 0)
          currentPrice = records.data.last.price;
      });
      
    } on Exception catch (e) {
      print(e);
      setState(() {
        records = charts.Series<StonksRecord, DateTime>(
          id: 'Records',
          colorFn: (_, __) => charts.MaterialPalette.blue.shadeDefault,
          domainFn: (StonksRecord records, _) => records.time,
          measureFn: (StonksRecord records, _) => records.price,
          data: []
        );
      });
    }
  }
}

class StonksRecord {
  final DateTime time;
  final int price;

  StonksRecord({this.time, this.price});
}