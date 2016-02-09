@for /r /d %%p in (bin) do @rd /s/q %%p 2>NUL
@for /r /d %%p in (obj) do @rd /s/q %%p 2>NUL
@for /r /d %%p in (x86) do @rd /s/q %%p 2>NUL
