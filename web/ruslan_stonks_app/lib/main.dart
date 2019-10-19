import 'dart:convert';

import 'package:auto_size_text/auto_size_text.dart';
import 'package:flutter/material.dart';
import 'package:charts_flutter/flutter.dart' as charts;
import 'package:http/http.dart' as http;



void main() => runApp(MyApp());

class MyApp extends StatelessWidget {

  final groupTokens = [
    "bramsdockercomposecirclejerk",
     "6d13ae8891a08cf4599f352720fb6e55",
    "2368464b4aa94530a9bfa8ed05e561bc",
     "987b3bdadd7d3ad28e7ddeb7d817fe55",
     "cee1684434ffe6e5d2fb8f491b143ca4",
     "1f28ca9c2ed88cf594e53a98bcc02955",
     "e90159c7a49f5deff4506a586a4dcda9",
     "084b86c1651470d24f65ff2d1c14b322",
     "2876f61f82fbf4768247cef16f0c28c8",
     "a05cfb802ce1819bfb63794cc53ba088",
     "815c1284aa04df30dcf4b1ab7b4c9a7a",
     "ebc03899163a9d8c86c0a5159353d424"
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
            // Expanded(child: MarketOverview(interval: Duration(minutes: 5), rate: Duration(seconds: 1))),
            Expanded(
              child: GridView.count(
                crossAxisCount: 5,
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
            alignment: FractionalOffset(2.0, 2.0),
            decoration: BoxDecoration(
              border: Border.all(
                color: (info["stonk_count"] ?? 0) > 0 ? Colors.blue.withOpacity(0.5) : Colors.grey.withOpacity(0.5),
                width: 5.0,
              ),
              shape: BoxShape.circle
            ),
            child: Center(
              child: Column(
                mainAxisAlignment: MainAxisAlignment.center,
                children: <Widget>[
                  AutoSizeText(info == null ? "?" : info["name"] ?? "?", maxLines: 1, style: TextStyle(fontSize: 24, fontWeight: FontWeight.bold)),
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
    var earnings = (info["total_value"] ?? 0) - 100000;
    var percentageEarnings = (earnings / 100000.0) * 100;
    var earningsString = "${earnings >= 0 ? '+' : ''} ${percentageEarnings.toStringAsFixed(2)}";


    return Padding(
      padding: const EdgeInsets.all(8.0),
      child: AutoSizeText("$earningsString%", maxLines: 1, style: TextStyle(fontSize: 28, color: earnings < 0 ? Colors.red : Colors.green)),
    );
  }

  Widget buildInfo() {
    return Column(
      children: <Widget>[
        AutoSizeText("Total Value", maxLines: 1, style: TextStyle(fontWeight: FontWeight.bold)),
        AutoSizeText("\$${info['total_value'] ?? 0}", maxLines: 1),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          children: <Widget>[
            Column(
              children: <Widget>[
                AutoSizeText("Stonks", maxLines: 1, style: TextStyle(fontWeight: FontWeight.bold)),
                AutoSizeText("\$${info['stonk_value'] ?? 0}", maxLines: 1)
              ],
            ),
            Column(
              children: <Widget>[
                AutoSizeText("Balance", maxLines: 1, style: TextStyle(fontWeight: FontWeight.bold)),
                AutoSizeText("\$${info['balance'] ?? 0}", maxLines: 1)
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
    var url = "http://172.17.68.206:3000/api/v1/account";

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
      child: Padding(
        padding: const EdgeInsets.all(8.0),
        child: StonksMarketChart(animate: true, interval: interval, rate: rate),
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
        // Expanded(
        //   child: charts.TimeSeriesChart(
            
        //     records == null ? [] : [records],
        //     animate: widget.animate,
        //     dateTimeFactory: const charts.LocalDateTimeFactory(),
        //     ),
        // )
      ],
    );
  }

  Widget headline() {
    var difference = currentPrice - lastPrice;
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      children: <Widget>[
        AutoSizeText("RUSLAN Stonks - Ligma Inc.        ", maxLines: 1, style: Theme.of(context).textTheme.headline),
        AutoSizeText("\$$currentPrice.00", maxLines: 1, style: TextStyle(
          fontSize: 22, 
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

    var url = "http://172.17.68.206:3000/api/v1/market?from=${from.toIso8601String()}&to=${to.toIso8601String()}";
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
          measureLowerBoundFn: (StonksRecord records, _) => records.price - 5,
          measureUpperBoundFn: (StonksRecord records, _) => records.price + 5,
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
