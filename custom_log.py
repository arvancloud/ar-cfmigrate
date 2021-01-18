import logging
from color import color
class SpecialFormatter(logging.Formatter):
    FORMATS = {logging.DEBUG: color.CYAN+"[DEBUG] - %(message)s - %(asctime)s"+color.END,
               logging.WARNING: color.YELLOW+"[WARNING] - %(message)s - %(asctime)s"+color.END,
               logging.ERROR: color.RED+"[ERROR] - %(message)s - %(asctime)s"+color.END,
               logging.INFO: color.GREEN+"[INFO] - %(message)s - %(asctime)s"+color.END,
               'DEFAULT': "%(levelname)s: %(message)s"}

    def format(self, record):
        self._fmt = self.FORMATS.get(record.levelno, self.FORMATS['DEFAULT'])
        return logging.Formatter.format(self, record)