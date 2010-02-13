; Media Keys Emulation

#InstallKeybdHook ; Only for viewing key history

#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.

SendMediaCommand(mediaKey) {
  Send, {Launch_Media}
  Sleep, 200
  Send, %mediaKey%
}

#Left::SendMediaCommand("{Media_Prev}")
#Right::SendMediaCommand("{Media_Next}")
#Up::SendMediaCommand("{Media_Play_Pause}")
#Down::Send, {Volume_Mute}
#'::Send, {Volume_Up}
#/::Send, {Volume_Down}


;VK  SC	Type	Up/Dn	Elapsed	Key		
;------------------------------------------------
;AD  120	a	d	13.59	D  Mute          	
;B2  124	a	d	0.25	J  Stop      	
;B1  110	a	d	0.33	Q  Prev        	
;B0  119	a	d	0.31	P  Next        	
;B3  122	a	d	0.22	G  Play/Pause        	
;B5  16D	a	d	2.78	Launch_Media   	
