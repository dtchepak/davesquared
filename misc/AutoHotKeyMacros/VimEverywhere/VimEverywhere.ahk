;
; AutoHotkey Version: 1.x
; Language:       English
; Author:         David <tchepak@gmail.com>
;
; Script Function:
;   Provides an Vim-like keybinding emulation mode that can be toggled on and off using
;   the CapsLock key.
;


;==========================
;Initialise
;==========================

#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.

IsEnabled := false
SetVimMode(false)

;==========================
;Functions
;==========================
SetVimMode(toActive) {
  ;local iconFile := toActive ? enabledIcon : disabledIcon
  local state := toActive ? "ON" : "OFF"

  IsEnabled := toActive
  TrayTip, Vim Everywhere, Vim mode is %state%, 10, 1
  ;Menu, Tray, Icon, %iconFile%,
  Menu, Tray, Tip, Vim Everywhere`nVim mode is %state%  

  Send {Shift Up}
}

SendCommand(key, translationToWindowsKeystrokes, secondWindowsKeystroke="") {
  global IsEnabled
  if (IsEnabled) {
    Send, %translationToWindowsKeystrokes%
    if (secondWindowsKeystroke<>"") {
      Send, %secondWindowsKeystroke%
    }
  } else {
    Send, %key% ;passthrough original keystroke
  }
  return
}

;==========================
;Vim mode toggle
;==========================

CapsLock::
  SetVimMode(!IsEnabled)
return

;==========================
;Character navigation
;==========================

$k::SendCommand("k","{Up}")
$j::SendCommand("j","{Down}")
$l::SendCommand("l","{Right}")
$h::SendCommand("h","{Left}")

;==========================
;Word Navigation
;==========================

$w::SendCommand("w","^{Right}")
$b::SendCommand("b","^{Left}")
$e::SendCommand("e","^{Right}","{Left}")

;==========================
;Line Navigation
;==========================

$0::SendCommand("0","{End}")
$$::SendCommand("$","{Home}")

;==========================
;Page Navigation
;==========================

$+G::SendCommand("G","^{End}")

;==========================
;Undo
;==========================

$u::SendCommand("u","^z")

;==========================
;Killing and Deleting
;==========================

$x::SendCommand("x","{Delete}")
$+X::SendCommand("X","{Backspace}")
$p::SendCommand("p","+{Insert}") ;paste
