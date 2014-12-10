import Novella_Parser
import requests

base_url = "http://shakespeare.mit.edu/"

books = ["allswell",
"asyoulikeit",
"comedy_errors",
"cymbeline",
"lll",
"measure",
"merry_wives",
"merchant",
"midsummer",
"much_ado",
"pericles",
"taming_shrew",
"tempest",
"troilus_cressida",
"twelfth_night",
"two_gentlemen",
"winters_tale",
"1henryiv",
"2henryiv",
"henryv",
"1henryvi",
"2henryvi",
"3henryvi",
"henryviii",
"john",
"richardii",
"richardiii",
"cleopatra",
"coriolanus",
"hamlet",
"julius_caesar",
"lear",
"macbeth",
"othello",
"romeo_juliet",
"timon",
"titus"]

def get_lines(txt):
    text = txt.splitlines()
    start_ind = 0
    end_ind = text.index("</body>")
    return text[start_ind:end_ind]

def main():
    
    for book in books:
        
        print("Getting " + book)
        
        r = requests.get(base_url + book + "/full.html")
        
        text = get_lines(r.text)
        
        Novella_Parser.parse_book(book, text)
    
    print "Done :)"

def test():
    r = requests.get(base_url + "allswell" + "/full.html")

    text = get_lines(r.text)
    #print(text[21:ind])
    
    Novella_Parser.parse_book("allswell", text)
    
if __name__ == "__main__":
    #test()
    main()
    
