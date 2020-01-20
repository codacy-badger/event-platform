import 'package:flutter/material.dart';

class HeroDialogRoute<T> extends PageRoute<T> {
    HeroDialogRoute({ this.builder }) : super();

    final WidgetBuilder builder;

    @override
    bool get opaque => false;

    @override
    bool get barrierDismissible => true;

    @override
    Duration get transitionDuration => const Duration(milliseconds: 300);

    @override
    bool get maintainState => true;

    @override
    Color get barrierColor => Color.fromARGB(1, 255, 255, 255);

    @override
    Widget buildTransitions(BuildContext context, Animation<double> animation, Animation<double> secondaryAnimation, Widget child) {
        return new FadeTransition(
            opacity: new CurvedAnimation(
                parent: animation,
                curve: Curves.easeOut
            ),
            child: child
        );
    }

    @override
    Widget buildPage(BuildContext context, Animation<double> animation,
        Animation<double> secondaryAnimation) {
        return builder(context);
    }

  @override
  String get barrierLabel => null;

}