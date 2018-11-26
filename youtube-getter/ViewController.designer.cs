// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace youtubegetter
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSButton audioCheck { get; set; }

		[Outlet]
		AppKit.NSButton audioCheckBox { get; set; }

		[Outlet]
		AppKit.NSTextField errorLabel { get; set; }

		[Outlet]
		AppKit.NSTextField infoLabelOne { get; set; }

		[Outlet]
		AppKit.NSTextField infoLabelTwo { get; set; }

		[Outlet]
		AppKit.NSTextField urlEntryBox { get; set; }

		[Outlet]
		AppKit.NSButton videoCheckBox { get; set; }

		[Action ("DownloadButtonClicked:")]
		partial void DownloadButtonClicked (Foundation.NSObject sender);

		[Action ("streamInfoClicked:")]
		partial void streamInfoClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (audioCheck != null) {
				audioCheck.Dispose ();
				audioCheck = null;
			}

			if (audioCheckBox != null) {
				audioCheckBox.Dispose ();
				audioCheckBox = null;
			}

			if (errorLabel != null) {
				errorLabel.Dispose ();
				errorLabel = null;
			}

			if (infoLabelOne != null) {
				infoLabelOne.Dispose ();
				infoLabelOne = null;
			}

			if (infoLabelTwo != null) {
				infoLabelTwo.Dispose ();
				infoLabelTwo = null;
			}

			if (urlEntryBox != null) {
				urlEntryBox.Dispose ();
				urlEntryBox = null;
			}

			if (videoCheckBox != null) {
				videoCheckBox.Dispose ();
				videoCheckBox = null;
			}
		}
	}
}
