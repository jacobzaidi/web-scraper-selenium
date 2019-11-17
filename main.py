from selenium import webdriver
from time import sleep
import datetime

"""
Web Scraper Selenium

This program extracts $1 bus deals from an inter city bus companies website and stores it in a text file.

Author: Jacob Zaidi
"""

from_city = input("Please state your origin: ")
to_city = input("Please state your destination: ")

driver = webdriver.Chrome(executable_path='chromedriver.exe')
driver.get("https://www.intercity.co.nz/")
sleep(3)

from_box = driver.find_element_by_id("BookTravelForm_getBookTravelForm_from")
to_box   = driver.find_element_by_id("BookTravelForm_getBookTravelForm_to")
sleep(1)

from_box.send_keys(from_city)
to_box.send_keys(to_city)
sleep(1)

when_box = driver.find_element_by_id("BookTravelForm_getBookTravelForm_date")
when_box.click()
sleep(2)

d = str(datetime.datetime.today().day + 1)
new_box = driver.find_element_by_xpath("//div[contains(text(), '" + d + "')]")
new_box.click()
sleep(3)

search_button = driver.find_element_by_id("BookTravelForm_getBookTravelForm_action_submit")
search_button.click()

year = "2019"

while True:
    f=open(from_city + to_city + ".txt", "a+")
    sleep(10)
    dates = driver.find_elements_by_xpath("//ul[@class='travel-dates js-show-ajax-end']/li")
    index = -1
    for i in range(len(dates)-1):
        if dates[i].get_attribute("class") == 'travel-date js-travel-date animate-cascade-down selected':
            index = i
            break
    sleep(2)
    today = dates[index].find_element_by_xpath(".//a").text
    tomorrow = dates[index+1].find_element_by_xpath(".//a")
    if year == 2019 and "Jan" in today:
        year = "2020"
    f.write(today + " " + year)
    prices = driver.find_elements_by_xpath("//div[contains(@class,'price')]/ancestor::form[contains(@method, 'post')]")
    for price in prices:
        fare = "SOLD OUT"
        try:
            fare = price.find_element_by_xpath(".//div[@class='price']").text
        except:
            fare = "SOLD OUT"
        times = price.find_elements_by_xpath(".//div[@class='fare-time']")
        if fare == "$1.00":
            f.write(" | " + times[0].text + " - " + times[1].text)
            print("*** $1 Fare ***")
            print(times[0].text," - ", times[1].text)
    f.write('\n')
    f.close()
    driver.get(tomorrow.get_attribute("href"))
