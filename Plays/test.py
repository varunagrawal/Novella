# Test file for Novella Parser

import requests
from Novella_Parser import *

p = NovellaHTMLParser()
p.test("Novella test starting")

r = requests.get("http://shakespeare.mit.edu/julius_caesar/full.html")
print "Got data"

l = r.text.split('\n')

p.feed(l[20])

