/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */
import {
  trigger,
  state,
  animate,
  transition,
  style,
  query,
  group
} from "@angular/animations";

export const fadeInAnimation =
  // trigger name for attaching this animation to an element using the [@triggerName] syntax
  trigger("fadeInAnimation", [
    // route 'enter' transition
    transition(":enter", [
      // css styles at start of transition
      style({ opacity: 0, background: "#ff0000" }),

      // animation and styles at end of transition
      animate(".8s", style({ opacity: 1 }))
    ])
  ]);
