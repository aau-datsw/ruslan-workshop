import 'dart:convert';
import 'dart:io';

import 'package:auto_size_text/auto_size_text.dart';
import 'package:flutter/material.dart';
import 'package:charts_flutter/flutter.dart' as charts;
import 'package:http/http.dart' as http;



void main() => runApp(MyApp());

class Configuration {
  static const PORT = String.fromEnvironment('RUSLAN_API_PORT');
  static const HOST = String.fromEnvironment('RUSLAN_API_HOST');
}

class MyApp extends StatelessWidget {

  final groupTokens = [
    "duendockercomposecirclejerk",
    "70c28e7e-5842-4f3f-a782-c19aa90323e4",
    "3973de15-2846-4a8a-9a1e-106df057a9bb",
    "635a89e1-2341-4578-993e-daecd54107f8",
    "9db642f2-6cbd-4f87-9f5c-fb60b5c12e25",
    "a9167c5d-6532-4ffd-b833-533ace4a02e3",
    "7189bf5e-4655-43a6-ae18-6714cb44a343",
    "311d82d1-9186-486b-8781-5e1f0902a87e",
    "5e3dcf74-a0fb-49eb-a984-0bb40be6ff3e",
    "1f374e33-c5ec-454e-8ffa-efd7bb368dd5",
    "b947810b-bf9f-4060-88ba-de0735783fa9",
    "4e2a9d55-1d06-477b-b972-199d8ac71d4f",
    "b713918a-6933-4d66-9e19-f44e587a959a",
    "491df023-9954-47bc-a63b-268f95c29f17",
    "6ad942b8-49c0-4e96-8d88-2471680d5a04",
    "f7f7b403-9c32-42df-88f9-d93a94eb7aa8"
  ];

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Flutter Demo',
      debugShowCheckedModeBanner: false,
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
                color: (info == null ? 0 : info["stonk_count"] ?? 0) > 0 ? Colors.blue.withOpacity(0.5) : Colors.grey.withOpacity(0.5),
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
    var earnings = (info == null ? 0 : info["total_value"] ?? 0) - 100000;
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
        AutoSizeText("\$${info == null ? 0 : info['total_value'] ?? 0}", maxLines: 1),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceAround,
          children: <Widget>[
            Column(
              children: <Widget>[
                AutoSizeText("Stonks", maxLines: 1, style: TextStyle(fontWeight: FontWeight.bold)),
                AutoSizeText("\$${info == null ? 0 : info['stonk_value'] ?? 0}", maxLines: 1)
              ],
            ),
            Column(
              children: <Widget>[
                AutoSizeText("Balance", maxLines: 1, style: TextStyle(fontWeight: FontWeight.bold)),
                AutoSizeText("\$${info == null ? 0 : info['balance'] ?? 0}", maxLines: 1)
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

  // Platform.environment['RUSLAN_API_PORT']

  void updateInfo(String xToken) async {
    var url = "http://${Configuration.HOST}:${Configuration.PORT}/api/v1/account";

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
        AutoSizeText("RUSLAN Stonks - Ligma Inc.        ", maxLines: 1),
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

    var url = "http://${Configuration.HOST}:${Configuration.PORT}/api/v1/market?from=${from.toIso8601String()}&to=${to.toIso8601String()}";
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
