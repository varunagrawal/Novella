#!/bin/python

from HTMLParser import HTMLParser
from htmlentitydefs import name2codepoint

import re
import io

file_data = []

class Dialogue:
    speaker = ""
    line = ""


def check_tag(end_tag):
    global file_data
    
    #if end_tag == "table" or end_tag == "head":
    #    print end_tag

    if end_tag == "h3" or end_tag == "H3":
        #print TagObject.tag_data    
        file_data.append("Banner#" + TagObject.tag_data)
    elif end_tag == "i":
        file_data.append("Notice#" + TagObject.tag_data)
    elif end_tag == "b":
        Dialogue.speaker = TagObject.tag_data
    elif end_tag == "a":
        if re.match("[0-9]+\.[0-9]+\.[0-9]+", TagObject.attr):
            Dialogue.line = TagObject.tag_data
            #print("Dialogue#"+ Dialogue.speaker + "=" + Dialogue.line)
            file_data.append("Dialogue#"+ Dialogue.speaker + "=" + Dialogue.line)

class TagObject:
    tag_type = ""
    tag_data = ""
    attr = ""

class NovellaHTMLParser(HTMLParser):
    
    """
        Clappics HTML Parser sub class
    """
    def test(self, s):
        print s
        
    def handle_starttag(self, tag, attrs):
        TagObject.tag_type = tag
        if len(attrs) > 0:
            TagObject.attr = attrs[0][1]
            
    def handle_endtag(self, tag):
        #print tag
        check_tag(tag)

    def handle_data(self, data):
        #print(TagObject.tag_type + data)
        TagObject.tag_data = data

def parse_book(filename, lines):
    parser = NovellaHTMLParser()
    
    global file_data
    file_data = []
    
    #filename = raw_input("Please enter file name:")
    
    #with io.open(filename, newline=None) as f:
    #    lines = f.readlines()

    #lines = text.splitlines()
    
    for line in lines:
        parser.feed(line.strip())

    with open("Formatted\\" + filename + ".txt", 'w') as op:
        d = "\n".join(file_data)
        op.write(d)

if __name__ == "__main__":
    parse_book("j_caesar.html")
