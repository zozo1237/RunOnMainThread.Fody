Imports RunOnMainThread
Imports RunOnMainThreadSample.Core

Class MainWindow
    Implements IDialogDisplayer
    Private ReadOnly DialogController As New DialogController(Me)

    Public Async Function RunOnBackgroundThread(Action As Action) As Task
        Dim WrapperTask As New Task(Action)
        WrapperTask.Start()
        Await WrapperTask
    End Function

    Private Async Sub OnShowDialog(Sender As Object, e As RoutedEventArgs)
        Await RunOnBackgroundThread(AddressOf DialogController.ShowDialog)
    End Sub

    Private Async Sub OnShowDialogUsingDispatcher(Sender As Object, e As RoutedEventArgs)
        Await RunOnBackgroundThread(AddressOf DialogController.ShowDialogUsingDispatcher)
    End Sub

    Private Async Sub OnShowDialogUsingWeaver(Sender As Object, e As RoutedEventArgs)
        Await RunOnBackgroundThread(AddressOf DialogController.ShowDialogUsingWeaver)
    End Sub

    Public Sub ShowDialog(text As String) Implements IDialogDisplayer.ShowDialog
        MessageBox.Show("On UI Thread: " & Dispatcher.CheckAccess)
    End Sub
End Class
