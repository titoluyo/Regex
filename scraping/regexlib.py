from lxml import html
import requests
import pymssql


def getValues(id):
    page = requests.get('http://regexlib.com/REDetails.aspx?regexp_id=' + str(id))
    tree = html.fromstring(page.content)

    title = tree.xpath('//span[@id="ctl00_ContentPlaceHolder1_TitleLabel"]/text()')
    expression = tree.xpath('//span[@id="ctl00_ContentPlaceHolder1_ExpressionLabel"]/text()')
    description = tree.xpath('//span[@id="ctl00_ContentPlaceHolder1_DescriptionLabel"]/text()')
    matched = tree.xpath('//span[@id="ctl00_ContentPlaceHolder1_MatchesLabel"]/text()')
    nonmatched = tree.xpath('//span[@id="ctl00_ContentPlaceHolder1_NonMatchesLabel"]/text()')
    if len(title) == 1:
        title = title[0]
    else:
        title = ''
    if len(expression) == 1:
        expression = expression[0]
    else:
        expression = ''
    if len(description) == 1:
        description = description[0]
    else:
        description = ''
    if len(matched) == 1:
        matched = matched[0]
    else:
        matched = ''
    if len(nonmatched) == 1:
        nonmatched = nonmatched[0]
    else:
        nonmatched = ''

    return title, expression, description, matched, nonmatched

conn = pymssql.connect(
    server='localhost',
    user='tluyo',
    password='karlita0803'
)
cursor = conn.cursor()

for i in range(127, 6000):
    print('...getting:{}'.format(i))
    title, expression, description, matched, nonmatched = getValues(i)
    if expression != '':
        # print('{}:{},{}'.format(i, title, expression))
        sql = "INSERT INTO RegexLib VALUES ({},'{}','{}','{}','{}','{}')".format(
                i, title.replace("'","''"), expression.replace("'","''"), description.replace("'","''"), matched.replace("'","''"), nonmatched.replace("'","''")
            )
        cursor.execute(sql)
        conn.commit()

conn.close()

