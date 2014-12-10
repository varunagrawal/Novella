def main():
    with open("colors.txt", 'w') as f:
        for r in range(256):
            for g in range(256):
                for b in range(256):
                    s = "#%02x%02x%02x" % (r, g, b)
                    print(s)
                    luminance = 0.2126*r + 0.7152*g + 0.0722*b
                    f.write("<div style='background-color:{0}'>Luminance:{1}</div>\n".format(s, luminance))
                    
if __name__ == "__main__":
    main()